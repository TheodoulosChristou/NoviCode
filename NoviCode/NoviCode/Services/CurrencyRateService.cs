using NoviCode.DTO;
using NoviCode.Entities;
using System.Xml.Linq;

namespace NoviCode.Services
{
    public class CurrencyRateService : ICurrencyRateService
    {
        private const string ECB_URL = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";

        private readonly NoviCodeDbContext.NoviCodeDbContext _dbContext;

        public CurrencyRateService(NoviCodeDbContext.NoviCodeDbContext dbContext) { 
            _dbContext = dbContext;
        }

        public async Task<List<CurrencyRateDto>> GetAllCurrencyRates()
        {
            using var client = new HttpClient();
            var response = await client.GetStringAsync(ECB_URL);

            var xdoc = XDocument.Parse(response);
            XNamespace ns = "http://www.ecb.int/vocabulary/2002-08-01/eurofxref";

            var date = DateTime.Parse(
                xdoc.Descendants(ns + "Cube")
                    .FirstOrDefault(x => x.Attribute("time") != null)?
                    .Attribute("time")?.Value
            );

            var rates = xdoc.Descendants(ns + "Cube")
                            .Where(x => x.Attribute("currency") != null)
                            .Select(x => new CurrencyRateDto
                            {
                                Currency = x.Attribute("currency")!.Value,
                                Rate = decimal.Parse(x.Attribute("rate")!.Value, System.Globalization.CultureInfo.InvariantCulture),
                                Date = date
                            }).ToList();

            rates.Add(new CurrencyRateDto
            {
                Currency = "EUR",
                Rate = 1.0m,
                Date = date
            });

            return rates;
        }


        public async Task<CurrencyRateDto> GetLatestRateByCurrencyAsync(string currency)
        {
            try
            {
                return  _dbContext.CurrencyRate
                                        .Where(r => r.CurrencyCode == currency)
                                        .OrderByDescending(r => r.RateDate)
                                        .Select(r => new CurrencyRateDto
                                        {
                                            Currency = r.CurrencyCode,
                                            Rate = r.Rate,
                                            Date = r.RateDate
                                        }).FirstOrDefault();


            }catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
