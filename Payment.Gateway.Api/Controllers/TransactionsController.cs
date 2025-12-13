using Microsoft.AspNetCore.Mvc;
using Payment.Gateway.DTOs;
using Payment.Gateway.Services;
using Payment.Gateway.Services.Common;

namespace Payment.Gateway.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IPaymentService _service;

        public TransactionsController(IPaymentService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTransactionReq req)
        {
            if (req == null) return BadRequest();
            var created = await _service.CreateTransactionAsync(req);
            return CreatedAtAction(nameof(GetTransactionByTNumber), new { transactionNumber = created.TransactionNumber }, created);
        }

        [HttpGet("by-transaction-number/{transactionNumber}")]
        public async Task<IActionResult> GetTransactionByTNumber(string transactionNumber)
        {
            var t = await _service.GetTransactionByTNumberAsync(transactionNumber);
            if (t == null) return NotFound();
            return Ok(t);
        }

        [HttpGet("by-id/{id:guid}")]
        public async Task<IActionResult> GetTransactionById(Guid id)
        {
            var t = await _service.GetTransactionByIdAsync(id);
            if (t == null) return NotFound();
            return Ok(t);
        }

        [HttpGet("by-order-id/{orderId:guid}")]
        public async Task<IActionResult> GetTransactionByOrderId(Guid orderId)
        {
            var t = await _service.GetTransactionByOrderIdAsync(orderId);
            if (t == null) return NotFound();
            return Ok(t);
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] TransactionListRequestDto request)
        {
            PageListRequestValidator.Normalize(request);
            var result = await _service.GetTransactionsAsync(request);
            return Ok(result);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusDto dto)
        {
            var updated = await _service.UpdateStatusAsync(id, dto.Status, dto.ProviderReference);
            if (updated == null) return NotFound();
            return Ok(updated);
        }
    }
}
