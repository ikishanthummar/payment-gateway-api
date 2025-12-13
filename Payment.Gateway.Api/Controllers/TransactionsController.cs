using Microsoft.AspNetCore.Mvc;
using Payment.Gateway.DTOs;
using Payment.Gateway.Services.Common;
using Payment.Gateway.Services.Interface;

namespace Payment.Gateway.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _service;

        public TransactionsController(ITransactionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] TransactionListRequestDto request)
        {
            PageListRequestValidator.Normalize(request);
            var result = await _service.GetTransactionsAsync(request);
            return Ok(result);
        }

        //[HttpGet("by-transaction-number/{transactionNumber}")]
        //public async Task<IActionResult> GetTransactionByTNumber(string transactionNumber)
        //{
        //    var t = await _service.GetTransactionByTNumberAsync(transactionNumber);
        //    if (t == null) return NotFound();
        //    return Ok(t);
        //}

        //[HttpGet("by-id/{id:guid}")]
        //public async Task<IActionResult> GetTransactionById(Guid id)
        //{
        //    var t = await _service.GetTransactionByIdAsync(id);
        //    if (t == null) return NotFound();
        //    return Ok(t);
        //}

        //[HttpGet("by-order-id/{orderId:guid}")]
        //public async Task<IActionResult> GetTransactionByOrderId(Guid orderId)
        //{
        //    var t = await _service.GetTransactionByOrderIdAsync(orderId);
        //    if (t == null) return NotFound();
        //    return Ok(t);
        //}
    }
}
