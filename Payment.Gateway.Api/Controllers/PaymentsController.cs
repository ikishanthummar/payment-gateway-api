using Microsoft.AspNetCore.Mvc;
using Payment.Gateway.DTOs.Payments;
using Payment.Gateway.Services.Interface;

namespace Payment.Gateway.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IWebhookService _webhookService;

        public PaymentsController(IPaymentService paymentService,
                                  IWebhookService webhookService)
        {
            _paymentService = paymentService;
            _webhookService = webhookService;
        }

        [HttpPost("initiate")]
        public async Task<IActionResult> InitiatePayment([FromBody] PaymentInitiateRequest req)
        {
            if (req == null) return BadRequest();
            var result = await _paymentService.InitiatePaymentAsync(req);
            return Ok(result);
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
