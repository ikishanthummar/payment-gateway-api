using Microsoft.IO;
using System.Security.Cryptography;
using System.Text;

namespace Payment.Gateway.Api.Middleware
{
    public class HmacVerificationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _secret;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager = new();

        public HmacVerificationMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _secret = config["PaymentGateway:WebhookSecret"];
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/callbacks"))
            {
                context.Request.EnableBuffering();
                using var ms = _recyclableMemoryStreamManager.GetStream();
                await context.Request.Body.CopyToAsync(ms);
                ms.Position = 0;
                var bodyAsText = new StreamReader(ms).ReadToEnd();
                ms.Position = 0;
                context.Request.Body.Position = 0;

                var headerSignature = context.Request.Headers["X-Signature"].ToString();
                if (string.IsNullOrEmpty(headerSignature))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Missing signature");
                    return;
                }

                var computed = ComputeHmacSha256(_secret, bodyAsText);
                if (!TimingsEqual(computed, headerSignature))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid signature");
                    return;
                }
            }

            await _next(context);
        }

        private static string ComputeHmacSha256(string secret, string payload)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secret ?? "");
            using var hmac = new HMACSHA256(keyBytes);
            var payloadBytes = Encoding.UTF8.GetBytes(payload ?? "");
            var hash = hmac.ComputeHash(payloadBytes);
            return Convert.ToHexString(hash).ToLower(); 
        }

        private static bool TimingsEqual(string a, string b)
        {
            if (a == null || b == null) return false;
            var aBytes = Encoding.UTF8.GetBytes(a);
            var bBytes = Encoding.UTF8.GetBytes(b);
            if (aBytes.Length != bBytes.Length) return false;
            var result = 0;
            for (int i = 0; i < aBytes.Length; i++) result |= aBytes[i] ^ bBytes[i];
            return result == 0;
        }
    }
}
