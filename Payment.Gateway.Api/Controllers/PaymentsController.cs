using Microsoft.AspNetCore.Mvc;
using Payment.Gateway.DTOs.Payments;
using Payment.Gateway.Services.Interface;

namespace Payment.Gateway.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _service;

        public PaymentsController(IPaymentService service)
        {
            _service = service;
        }

        [HttpPost("initiate")]
        public async Task<IActionResult> InitiatePayment([FromBody] PaymentInitiateRequestDto req)
        {
            if (req == null) return BadRequest();
            var result = await _service.InitiatePaymentAsync(req);
            return Ok(result);
        }
    }
}
