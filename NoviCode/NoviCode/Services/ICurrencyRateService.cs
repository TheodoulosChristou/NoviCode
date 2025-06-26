using NoviCode.DTO;
using NoviCode.Entities;

namespace NoviCode.Services
{
    public interface ICurrencyRateService
    {
        public Task<List<CurrencyRateDto>> GetAllCurrencyRates();

        public Task<CurrencyRateDto> GetLatestRateByCurrencyAsync(string currency);
    }
}
