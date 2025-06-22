using NoviCode.Entities;
using System.Xml.Linq;

namespace NoviCode.Services
{
    public class CurrencyRateService : ICurrencyRateService
    {
        private const string ECB_URL = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";

        public CurrencyRateService() { 
        
        }
        public async Task<List<CurrencyRate>> GetAllCurrencyRates()
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.GetStringAsync(ECB_URL);

                var xdoc = XDocument.Parse(response);

                // Define the default namespace used in the XML
                XNamespace ns = "http://www.ecb.int/vocabulary/2002-08-01/eurofxref";

                var rates = xdoc
                    .Descendants(ns + "Cube")
                    .Where(x => x.Attribute("currency") != null && x.Attribute("rate") != null)
                    .Select(x => new CurrencyRate
                    {
                        Currency = x.Attribute("currency")?.Value,
                        Rate = decimal.Parse(x.Attribute("rate")?.Value)
                    })
                    .ToList();

                return rates;
            } catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
