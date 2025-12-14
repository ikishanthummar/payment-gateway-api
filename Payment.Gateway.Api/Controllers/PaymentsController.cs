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

        /// <summary>
        /// Get list of available payment providers.
        /// </summary>
        /// <remarks>
        /// This endpoint is used to populate the payment provider dropdown
        /// on the payment initiation screen.
        /// </remarks>
        /// <response code="200">Returns list of payment providers</response>
        [HttpGet("payment-providers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaymentProvidersDDLAsync()
        {
            var result = await _paymentService.GetPaymentProvidersDDLAsync();
            return Ok(result);
        }

        /// <summary>
        /// Initiate a new payment.
        /// </summary>
        /// <remarks>
        /// Creates a new payment request and generates a pending transaction.
        /// The returned response contains order and transaction details
        /// which are later updated via webhook callbacks.
        /// </remarks>
        /// <param name="req">Payment initiation request payload</param>
        /// <response code="200">Payment initiated successfully</response>
        /// <response code="400">Invalid request payload</response>
        [HttpPost("initiate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InitiatePayment([FromBody] PaymentInitiateRequest req)
        {
            if (req == null) return BadRequest();
            var result = await _paymentService.InitiatePaymentAsync(req);
            return Ok(result);
        }
    }
}
