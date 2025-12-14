using Microsoft.IO;
using System.Security.Cryptography;
using System.Text;

namespace Payment.Gateway.Api.Middleware
{
    public class WebhookHmacVerificationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _secret;
        private readonly int _toleranceMinutes;
        private readonly RecyclableMemoryStreamManager _streamManager = new();

        public WebhookHmacVerificationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _secret = configuration["PaymentGateway:WebhookSecret"] ?? throw new InvalidOperationException("Webhook secret is not configured");
            _toleranceMinutes = configuration.GetValue("PaymentGateway:WebhookToleranceMinutes", 10);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Path.StartsWithSegments("/api/webhook/callback"))
            {
                await _next(context);
                return;
            }

            if (!TryValidateHeaders(context, out var signature, out var timestamp, out var eventId))
                return;

            
            var rawBody = await ReadRawBodyAsync(context);

            if (!IsSignatureValid(rawBody, timestamp, signature))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid signature");
                return;
            }

            context.Items["RawWebhookBody"] = rawBody;
            context.Items["WebhookEventId"] = eventId;

            await _next(context);
        }

        #region Private methods
        private bool TryValidateHeaders(HttpContext context, out string signature, out long timestamp, out string eventId)
        {
            signature = context.Request.Headers["X-Signature"];
            eventId = context.Request.Headers["X-Event-Id"];

            if (!long.TryParse(context.Request.Headers["X-Timestamp"], out timestamp) ||
                string.IsNullOrWhiteSpace(signature) ||
                string.IsNullOrWhiteSpace(eventId))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.WriteAsync("Missing or invalid webhook headers").Wait();
                return false;
            }

            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            if (Math.Abs(now - timestamp) > _toleranceMinutes * 60)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.WriteAsync("Webhook request expired").Wait();
                return false;
            }

            return true;
        }

        private async Task<string> ReadRawBodyAsync(HttpContext context)
        {
            context.Request.EnableBuffering();

            using var stream = _streamManager.GetStream();
            await context.Request.Body.CopyToAsync(stream);
            stream.Position = 0;

            var body = await new StreamReader(stream).ReadToEndAsync();
            context.Request.Body.Position = 0;

            return body;
        }

        private bool IsSignatureValid(string rawBody, long timestamp, string signature)
        {
            var payload = $"{timestamp}.{rawBody}";
            var computed = ComputeHmacSha256(payload, _secret);

            return CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(computed), Encoding.UTF8.GetBytes(signature));
        }

        private static string ComputeHmacSha256(string payload, string secret)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            return Convert.ToHexString(hash).ToLowerInvariant();
        } 
        #endregion
    }
}
