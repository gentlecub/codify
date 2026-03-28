using System.Xml;
using System.Globalization;
using AssetTracking.Models;

namespace AssetTracking.Services
{
    public static class CurrencyService
    {
        private static List<CurrencyObj> _currencyList = new List<CurrencyObj>();
        private static bool _isInitialized = false;

        // Fetch exchange rates from European Central Bank
        public static void FetchRates()
        {
            if (_isInitialized) return;

            try
            {
                string url = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";

                XmlTextReader reader = new XmlTextReader(url);
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        while (reader.MoveToNextAttribute())
                        {
                            if (reader.Name == "currency")
                            {
                                string currencyCode = reader.Value;
                                reader.MoveToNextAttribute();

                                // Use InvariantCulture for parsing decimal with dot separator
                                double rate = double.Parse(reader.Value, CultureInfo.InvariantCulture);
                                _currencyList.Add(new CurrencyObj(currencyCode, rate));
                            }
                        }
                    }
                }

                _isInitialized = true;
                Console.WriteLine($"Currency rates loaded successfully. {_currencyList.Count} currencies available.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Could not fetch live rates. Using fallback rates.");
                Console.WriteLine($"Error: {ex.Message}");

                // Fallback rates in case API is unavailable
                _currencyList.Add(new CurrencyObj("USD", 1.08));
                _currencyList.Add(new CurrencyObj("SEK", 11.20));
                _isInitialized = true;
            }
        }

        // Convert from one currency to another via EUR
        public static decimal Convert(decimal input, string fromCurrency, string toCurrency)
        {
            if (!_isInitialized)
            {
                FetchRates();
            }

            // Same currency - no conversion needed
            if (fromCurrency == toCurrency)
            {
                return input;
            }

            double value = (double)input;

            // Step 1: Convert to EUR
            if (fromCurrency == "EUR")
            {
                // Already in EUR, no conversion needed
            }
            else
            {
                var fromRate = _currencyList.Find(c => c.CurrencyCode == fromCurrency);
                if (fromRate != null)
                {
                    value = value / fromRate.ExchangeRateFromEUR;
                }
            }

            // Step 2: Convert from EUR to target currency
            if (toCurrency == "EUR")
            {
                // Target is EUR, already converted
            }
            else
            {
                var toRate = _currencyList.Find(c => c.CurrencyCode == toCurrency);
                if (toRate != null)
                {
                    value = value * toRate.ExchangeRateFromEUR;
                }
            }

            return (decimal)Math.Round(value, 2);
        }

        // Get available currencies
        public static List<string> GetAvailableCurrencies()
        {
            var currencies = _currencyList.Select(c => c.CurrencyCode).ToList();
            currencies.Insert(0, "EUR");
            return currencies;
        }
    }
}
