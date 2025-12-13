using Microsoft.AspNetCore.Mvc;
using Payment.Gateway.DTOs;
using Payment.Gateway.Services;

namespace Payment.Gateway.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CallbacksController : ControllerBase
    {
        private readonly IPaymentService _service;

        public CallbacksController(IPaymentService service)
        {
            _service = service;
        }

        [HttpPost("payment")]
        public async Task<IActionResult> PaymentCallback([FromBody] ProviderCallbackDto dto)
        {
            if (dto == null) return BadRequest();

            if (!Guid.TryParse(dto.TransactionId, out var guid)) {}

            var updated = await _service.UpdateStatusAsync(guid, dto.Status, dto.ProviderReference);
            if (updated == null) return NotFound();

            return Ok(new { success = true });
        }
    }
}
