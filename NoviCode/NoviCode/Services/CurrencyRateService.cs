using NoviCode.DTO;
using NoviCode.Entities;
using System.Xml.Linq;

namespace NoviCode.Services
{
    public class CurrencyRateService : ICurrencyRateService
    {
        private const string ECB_URL = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";

        public CurrencyRateService() { 
        
        }
        public async Task<List<CurrencyRateDto>> GetAllCurrencyRates()
        {
            using var client = new HttpClient();
            var response = await client.GetStringAsync("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");

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
                                Currency = x.Attribute("currency").Value,
                                Rate = decimal.Parse(x.Attribute("rate").Value, System.Globalization.CultureInfo.InvariantCulture),
                                Date = date
                            }).ToList();

            return rates;
        }


    }
}
