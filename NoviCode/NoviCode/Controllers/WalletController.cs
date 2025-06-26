using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations;
using NoviCode.Entities;
using NoviCode.Services;

namespace NoviCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _service;

        public WalletController(IWalletService service)
        {
                _service = service;            
        }

        [HttpPost("CreateWallet")]
        public async Task<ActionResult<Wallet>> CreateWallet([FromBody]Wallet walletRequest)
        {
            var result = await _service.CreateWallet(walletRequest);    
            return Ok(result);
        }

        [HttpGet("{walletId}")]
        public async Task<IActionResult> GetWalletBalance(long walletId, [FromQuery] string? currency = null)
        {
            try
            {
                var result = await _service.RetrieveWalletBalance(walletId, currency);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("AdjustWallet")]
        public async Task<ActionResult<Wallet>> AdjustWallet(long walletId, [FromQuery]decimal amount, [FromQuery]string currency, [FromQuery]string strategy)
        {
            try
            {
                var result = await _service.AdjustWalletBalance(walletId, amount, currency, strategy);
                return Ok(result);
            } catch(Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
