using AssetTracking.Models;

namespace AssetTracking.Services
{
    public class AssetManager
    {
        private List<Asset> _assets = new List<Asset>();

        // Add a new asset and convert price to local currency
        public void AddAsset(Asset asset)
        {
            // Convert USD price to local currency
            asset.LocalPrice = CurrencyService.Convert(
                asset.PriceUSD,
                "USD",
                asset.Office.CurrencyCode
            );

            _assets.Add(asset);
            Console.WriteLine($"\nAsset added successfully!");
            Console.WriteLine($"Price: ${asset.PriceUSD:N2} USD = {asset.LocalPrice:N2} {asset.Office.CurrencyCode}");
        }

        // Print all assets sorted by office, then by purchase date
        public void PrintAssets()
        {
            if (_assets.Count == 0)
            {
                Console.WriteLine("\nNo assets registered yet.");
                return;
            }

            // Sort by office name, then by purchase date
            var sortedAssets = _assets
                .OrderBy(a => a.Office.Name)
                .ThenBy(a => a.PurchaseDate)
                .ToList();

            Console.WriteLine();
            PrintHeader();
            PrintSeparator();

            string currentOffice = "";

            foreach (var asset in sortedAssets)
            {
                // Print office header when office changes
                if (asset.Office.Name != currentOffice)
                {
                    if (currentOffice != "")
                    {
                        Console.WriteLine(); // Empty line between offices
                    }
                    currentOffice = asset.Office.Name;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"--- {asset.Office.Name} Office ({asset.Office.CurrencyCode}) ---");
                    Console.ResetColor();
                }

                // Set color based on end-of-life status
                Console.ForegroundColor = asset.GetStatusColor();
                Console.WriteLine(asset.ToString());
                Console.ResetColor();
            }

            PrintSeparator();
            PrintLegend();
        }

        private void PrintHeader()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{"Type",-10} | {"Brand",-12} | {"Model",-15} | {"Price",14} | {"Purchase",-10} | Status");
            Console.ResetColor();
        }

        private void PrintSeparator()
        {
            Console.WriteLine(new string('-', 80));
        }

        private void PrintLegend()
        {
            Console.WriteLine("\nLegend:");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("  RED");
            Console.ResetColor();
            Console.WriteLine(" = Less than 3 months until end of life");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("  YELLOW");
            Console.ResetColor();
            Console.WriteLine(" = Less than 6 months until end of life");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("  WHITE");
            Console.ResetColor();
            Console.WriteLine(" = More than 6 months until end of life");
        }

        // Get total asset count
        public int GetAssetCount() => _assets.Count;

        // Get assets by office
        public List<Asset> GetAssetsByOffice(string officeName)
        {
            return _assets.Where(a => a.Office.Name == officeName).ToList();
        }
    }
}
