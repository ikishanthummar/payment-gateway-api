using Microsoft.AspNetCore.Mvc;
using Payment.Gateway.DTOs.Payments;
using Payment.Gateway.Services.Interface;
using Swashbuckle.AspNetCore.Annotations;

namespace Payment.Gateway.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly IWebhookService _webhookService;

        public WebhookController(IWebhookService webhookService)
        {
            _webhookService = webhookService;
        }

        /// <summary>
        /// Payment provider webhook callback endpoint.
        /// </summary>
        /// <remarks>
        /// This endpoint is invoked by the payment provider to notify the system
        /// about payment status updates (Success / Failed).
        ///
        /// SECURITY REQUIREMENTS:
        /// 1. Request must contain the following headers:
        ///    - X-Signature : HMAC SHA256 signature
        ///    - X-Timestamp : Unix timestamp (seconds)
        ///    - X-Event-Id  : Unique webhook event identifier
        ///
        /// 2. Signature validation:
        ///    - Payload used for signature:
        ///      "{X-Timestamp}.{rawRequestBody}"
        ///    - HMAC SHA256 is computed using the configured webhook secret
        ///    - The computed signature must exactly match the X-Signature header
        ///
        /// 3. Replay attack protection:
        ///    - X-Timestamp must be within ±10 minutes (configurable)
        ///
        /// Requests failing validation will be rejected with 400 or 401 status codes.
        ///
        /// This endpoint is NOT intended for frontend or public usage.
        /// </remarks>
        /// <param name="dto">Webhook payload sent by the payment provider</param>
        /// <response code="200">Webhook processed successfully</response>
        /// <response code="400">Missing or invalid webhook headers or payload</response>
        /// <response code="401">Invalid signature or expired webhook request</response>
        [HttpPost("callback")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PaymentCallback([FromBody] PaymentCallbackRequest dto)
        {
            var rawBody = HttpContext.Items["RawWebhookBody"]?.ToString() ?? "";
            var signature = Request.Headers["X-Signature"].ToString();

            await _webhookService.ProcessPaymentCallbackAsync(dto, rawBody, signature);

            return Ok(new { message = "Callback processed" });
        }
    }
}
