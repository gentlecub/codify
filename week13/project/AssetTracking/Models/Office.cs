namespace AssetTracking.Models
{
    public class Office
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }

        public Office(string name, string country, string currencyCode, string currencySymbol)
        {
            Name = name;
            Country = country;
            CurrencyCode = currencyCode;
            CurrencySymbol = currencySymbol;
        }

        // Predefined offices
        public static List<Office> GetOffices()
        {
            return new List<Office>
            {
                new Office("USA", "United States", "USD", "$"),
                new Office("Spain", "Spain", "EUR", "€"),
                new Office("Sweden", "Sweden", "SEK", "kr")
            };
        }

        public override string ToString()
        {
            return $"{Name} ({CurrencyCode})";
        }
    }
}
