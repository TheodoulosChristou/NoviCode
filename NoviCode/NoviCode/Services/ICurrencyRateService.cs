using NoviCode.Entities;

namespace NoviCode.Services
{
    public interface ICurrencyRateService
    {
        public Task<List<CurrencyRate>> GetAllCurrencyRates();
    }
}
