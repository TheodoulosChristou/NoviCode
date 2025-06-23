using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoviCode.DTO;
using NoviCode.Entities;
using NoviCode.Services;
using NoviCode.SQLBuilder;

namespace NoviCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyRateController : ControllerBase
    {
        private readonly ICurrencyRateService _service;
        private readonly NoviCodeDbContext.NoviCodeDbContext _dbContext;

        public CurrencyRateController(ICurrencyRateService service, NoviCodeDbContext.NoviCodeDbContext dbContext)
        {
            _service  = service;
            _dbContext = dbContext;
        }

        [HttpGet("GetAllCurrencyRatesSync")]
        public async Task<ActionResult<List<CurrencyRateDto>>> GetAllCurrencyRatesSync()
        {
            var rates = await _service.GetAllCurrencyRates();
            if(rates.Count == 0)
            {
                return NotFound();
            } else
            {
                var mergeSql = MergeSqlBuilder.BuildMergeSql(rates);
                await _dbContext.Database.ExecuteSqlRawAsync(mergeSql);

                return Ok(rates);
            }
        }

        [HttpGet("GetAllCurrencyRates")]
        public async Task<ActionResult<List<CurrencyRate>>> GetAllCurrencyRates()
        {
            var result = await _service.GetAllCurrencyRates();
            return Ok(result);
        }
    }
}
