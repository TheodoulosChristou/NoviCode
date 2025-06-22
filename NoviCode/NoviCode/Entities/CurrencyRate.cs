namespace NoviCode.Entities
{
    public class CurrencyRate
    {
        public int Id { get; set; }

        public string CurrencyCode { get; set; }

        public decimal Rate { get; set; }

        public DateTime RateDate { get; set; }
    }
}
