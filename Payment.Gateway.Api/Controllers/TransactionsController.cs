using Microsoft.AspNetCore.Mvc;
using Payment.Gateway.DTOs.Transaction;
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

        /// <summary>
        /// Get paginated list of transactions.
        /// </summary>
        /// <remarks>
        /// This endpoint returns a filtered and paginated list of transactions.
        /// It supports searching, sorting, and pagination parameters via query string.
        /// </remarks>
        /// <param name="request">Transaction list request parameters</param>
        /// <response code="200">Returns paginated list of transactions</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> List([FromQuery] TransactionListRequestDto request)
        {
            PageListRequestValidator.Normalize(request);
            var result = await _service.GetTransactionsAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Get transaction details by transaction number.
        /// </summary>
        /// <remarks>
        /// This endpoint is used to fetch a single transaction using its
        /// unique transaction number (e.g. TXN-20251213-XXXX).
        /// Commonly used on payment status or transaction detail screens.
        /// </remarks>
        /// <param name="transactionNumber">Unique transaction number</param>
        /// <response code="200">Transaction found and returned</response>
        /// <response code="404">Transaction not found</response>
        [HttpGet("by-transaction-number/{transactionNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTransactionByTNumber(string transactionNumber)
        {
            var t = await _service.GetTransactionByTNumberAsync(transactionNumber);
            if (t == null) return NotFound();
            return Ok(t);
        }

        #region If Required
        ///// <summary>
        ///// Get transaction details by transaction ID.
        ///// </summary>
        ///// <remarks>
        ///// Fetches a single transaction using its unique internal transaction ID (GUID).
        ///// Typically used by admin panels or internal services.
        ///// </remarks>
        ///// <param name="id">Transaction unique identifier (GUID)</param>
        ///// <response code="200">Transaction found and returned</response>
        ///// <response code="404">Transaction not found</response>
        //[HttpGet("by-id/{id:guid}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> GetTransactionById(Guid id)
        //{
        //    var t = await _service.GetTransactionByIdAsync(id);
        //    if (t == null) return NotFound();
        //    return Ok(t);
        //}

        ///// <summary>
        ///// Get transaction details by order ID.
        ///// </summary>
        ///// <remarks>
        ///// Retrieves a transaction associated with a specific order ID.
        ///// This is commonly used after order creation to check
        ///// payment status or transaction details.
        ///// </remarks>
        ///// <param name="orderId">Order unique identifier (GUID)</param>
        ///// <response code="200">Transaction found and returned</response>
        ///// <response code="404">Transaction not found</response>
        //[HttpGet("by-order-id/{orderId:guid}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> GetTransactionByOrderId(Guid orderId)
        //{
        //    var t = await _service.GetTransactionByOrderIdAsync(orderId);
        //    if (t == null) return NotFound();
        //    return Ok(t);
        //}
        #endregion
    }
}
