using Microsoft.IO;
using System.Security.Cryptography;
using System.Text;

namespace Payment.Gateway.Api.Middleware
{
    public class WebhookHmacVerificationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _secret;
        private readonly RecyclableMemoryStreamManager _streamManager = new();

        public WebhookHmacVerificationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _secret = configuration["PaymentGateway:WebhookSecret"] ?? throw new InvalidOperationException("Webhook secret is not configured");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Path.StartsWithSegments("/api/webhook/callback"))
            {
                await _next(context);
                return;
            }

            context.Request.EnableBuffering();

            using var stream = _streamManager.GetStream();
            await context.Request.Body.CopyToAsync(stream);
            stream.Position = 0;

            var rawBody = await new StreamReader(stream).ReadToEndAsync();

            stream.Position = 0;
            context.Request.Body.Position = 0;

            context.Items["RawWebhookBody"] = rawBody;

            var headerSignature = context.Request.Headers["X-Signature"].ToString().Replace("sha256=", "", StringComparison.OrdinalIgnoreCase);

            if (string.IsNullOrWhiteSpace(headerSignature))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Missing signature");
                return;
            }

            var computedSignature = ComputeHmacSha256(rawBody, _secret);

            if (!TimingSafeEquals(computedSignature, headerSignature))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid signature");
                return;
            }

            await _next(context);
        }

        private static string ComputeHmacSha256(string payload, string secret)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            return Convert.ToHexString(hash).ToLowerInvariant();
        }

        private static bool TimingSafeEquals(string a, string b)
        {
            if (a.Length != b.Length) return false;

            var diff = 0;
            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];

            return diff == 0;
        }
    }
}
