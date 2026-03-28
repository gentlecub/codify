using AssetTracking.Enums;

namespace AssetTracking.Models
{
    public abstract class Asset
    {
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public decimal PriceUSD { get; set; }
        public decimal LocalPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Office Office { get; set; } = null!;

        // Abstract property - each derived class must implement
        public abstract string AssetTypeName { get; }

        // End of life is 3 years from purchase date
        public DateTime EndOfLife => PurchaseDate.AddYears(3);

        // Calculate months remaining until end of life
        public int MonthsUntilEndOfLife
        {
            get
            {
                var today = DateTime.Now;
                int months = ((EndOfLife.Year - today.Year) * 12) + (EndOfLife.Month - today.Month);

                // Adjust for day of month
                if (EndOfLife.Day < today.Day)
                    months--;

                return months;
            }
        }

        // Determine color based on time remaining
        public ConsoleColor GetStatusColor()
        {
            int monthsLeft = MonthsUntilEndOfLife;

            if (monthsLeft < 3)
                return ConsoleColor.Red;
            else if (monthsLeft < 6)
                return ConsoleColor.Yellow;
            else
                return ConsoleColor.White;
        }

        // Get status description
        public string GetStatusDescription()
        {
            int monthsLeft = MonthsUntilEndOfLife;

            if (monthsLeft < 0)
                return "EXPIRED";
            else if (monthsLeft < 3)
                return "CRITICAL";
            else if (monthsLeft < 6)
                return "WARNING";
            else
                return "OK";
        }

        public override string ToString()
        {
            return $"{AssetTypeName,-10} | {Brand,-12} | {Model,-15} | " +
                   $"{LocalPrice,10:N2} {Office.CurrencyCode} | " +
                   $"{PurchaseDate:yyyy-MM-dd} | {GetStatusDescription()}";
        }
    }
}
