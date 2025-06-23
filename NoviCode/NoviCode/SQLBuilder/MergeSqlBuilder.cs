using NoviCode.DTO;
using NoviCode.Entities;
using System.Text;

namespace NoviCode.SQLBuilder
{
    public class MergeSqlBuilder
    {

        public static string BuildMergeSql(List<CurrencyRateDto> rates)
        {
            var sb = new StringBuilder();
            sb.AppendLine("MERGE INTO CurrencyRate AS Target");
            sb.AppendLine("USING (VALUES");

            sb.AppendLine(string.Join(",\n", rates.Select(r =>
                $"('{r.Currency.Replace("'", "''")}', {r.Rate.ToString(System.Globalization.CultureInfo.InvariantCulture)}, '{r.Date:yyyy-MM-dd}')")));

            sb.AppendLine(") AS Source (CurrencyCode, Rate, RateDate)");
            sb.AppendLine("ON Target.CurrencyCode = Source.CurrencyCode AND Target.RateDate = Source.RateDate");
            sb.AppendLine("WHEN MATCHED THEN UPDATE SET Rate = Source.Rate");
            sb.AppendLine("WHEN NOT MATCHED THEN INSERT (CurrencyCode, Rate, RateDate) VALUES (Source.CurrencyCode, Source.Rate, Source.RateDate);");

            return sb.ToString();
        }

    }
}
