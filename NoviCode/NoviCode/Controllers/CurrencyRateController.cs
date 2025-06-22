using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoviCode.Entities;
using NoviCode.Services;

namespace NoviCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyRateController : ControllerBase
    {
        private readonly ICurrencyRateService _service;

        public CurrencyRateController(ICurrencyRateService service)
        {
            _service  = service;
        }

        [HttpGet("GetAllCurrencyRates")]
        public async Task<ActionResult<List<CurrencyRate>>> GetAllCurrencyRates()
        {
            var result = await _service.GetAllCurrencyRates();
            return Ok(result);
        }
    }
}
