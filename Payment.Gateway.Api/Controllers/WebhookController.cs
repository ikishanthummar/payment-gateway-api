using Microsoft.AspNetCore.Mvc;
using Payment.Gateway.DTOs.Payments;
using Payment.Gateway.Services.Interface;

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

        [HttpPost("callback")]
        public async Task<IActionResult> PaymentCallback([FromBody] PaymentCallbackRequest dto)
        {
            var rawBody = HttpContext.Items["RawWebhookBody"]?.ToString() ?? "";
            var signature = Request.Headers["X-Signature"].ToString();

            await _webhookService.ProcessPaymentCallbackAsync(dto, rawBody, signature);

            return Ok(new { message = "Callback processed" });
        }
    }
}
