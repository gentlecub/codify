using AssetTracking.Models;
using AssetTracking.Services;

namespace AssetTracking
{
    class Program
    {
        static AssetManager assetManager = new AssetManager();
        static List<Office> offices = Office.GetOffices();

        static void Main(string[] args)
        {
            Console.WriteLine("===========================================");
            Console.WriteLine("       ASSET TRACKING SYSTEM");
            Console.WriteLine("===========================================");
            Console.WriteLine();

            // Initialize currency service
            Console.WriteLine("Loading exchange rates...");
            CurrencyService.FetchRates();
            Console.WriteLine();

            // Add some sample data for testing
            AddSampleData();

            // Main menu loop
            bool running = true;
            while (running)
            {
                PrintMenu();
                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        AddNewAsset();
                        break;
                    case "2":
                        assetManager.PrintAssets();
                        break;
                    case "3":
                        running = false;
                        Console.WriteLine("\nGoodbye!");
                        break;
                    default:
                        Console.WriteLine("\nInvalid option. Please try again.");
                        break;
                }
            }
        }

        static void PrintMenu()
        {
            Console.WriteLine("\n=== MAIN MENU ===");
            Console.WriteLine("1. Add New Asset");
            Console.WriteLine("2. View All Assets");
            Console.WriteLine("3. Exit");
            Console.Write("\nSelect an option: ");
        }

        static void AddNewAsset()
        {
            Console.WriteLine("\n=== ADD NEW ASSET ===\n");

            // Select asset type
            Console.WriteLine("Select asset type:");
            Console.WriteLine("1. Laptop");
            Console.WriteLine("2. Computer");
            Console.WriteLine("3. Phone");
            Console.Write("Choice: ");

            string typeChoice = Console.ReadLine() ?? "1";

            // Select office
            Console.WriteLine("\nSelect office:");
            for (int i = 0; i < offices.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {offices[i]}");
            }
            Console.Write("Choice: ");

            if (!int.TryParse(Console.ReadLine(), out int officeIndex) || officeIndex < 1 || officeIndex > offices.Count)
            {
                Console.WriteLine("Invalid office selection.");
                return;
            }

            Office selectedOffice = offices[officeIndex - 1];

            // Get asset details
            Console.Write("\nBrand: ");
            string brand = Console.ReadLine() ?? "Unknown";

            Console.Write("Model: ");
            string model = Console.ReadLine() ?? "Unknown";

            Console.Write("Price in USD: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal priceUSD))
            {
                Console.WriteLine("Invalid price.");
                return;
            }

            Console.Write("Purchase date (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime purchaseDate))
            {
                Console.WriteLine("Invalid date format.");
                return;
            }

            // Create the appropriate asset type
            Asset asset = typeChoice switch
            {
                "1" => new Laptop(),
                "2" => new Computer(),
                "3" => new Phone(),
                _ => new Laptop()
            };

            // Set properties
            asset.Brand = brand;
            asset.Model = model;
            asset.PriceUSD = priceUSD;
            asset.PurchaseDate = purchaseDate;
            asset.Office = selectedOffice;

            // Add to manager
            assetManager.AddAsset(asset);
        }

        // Add sample data for demonstration
        static void AddSampleData()
        {
            Console.WriteLine("Adding sample data for demonstration...\n");

            // USA Office assets
            var usaOffice = offices[0]; // USA

            var laptop1 = new Laptop
            {
                Brand = "Apple",
                Model = "MacBook Pro 16",
                PriceUSD = 2499.00m,
                PurchaseDate = DateTime.Now.AddYears(-3).AddMonths(1), // RED - almost 3 years old
                Office = usaOffice
            };

            var phone1 = new Phone
            {
                Brand = "Apple",
                Model = "iPhone 15 Pro",
                PriceUSD = 999.00m,
                PurchaseDate = DateTime.Now.AddYears(-2).AddMonths(-8), // YELLOW - about 2.5 years old
                Office = usaOffice
            };

            // Spain Office assets
            var spainOffice = offices[1]; // Spain

            var computer1 = new Computer
            {
                Brand = "Dell",
                Model = "OptiPlex 7090",
                PriceUSD = 1200.00m,
                PurchaseDate = DateTime.Now.AddYears(-1), // WHITE - 1 year old
                Office = spainOffice
            };

            var laptop2 = new Laptop
            {
                Brand = "Lenovo",
                Model = "ThinkPad X1",
                PriceUSD = 1899.00m,
                PurchaseDate = DateTime.Now.AddYears(-3).AddMonths(2), // RED - almost expired
                Office = spainOffice
            };

            // Sweden Office assets
            var swedenOffice = offices[2]; // Sweden

            var phone2 = new Phone
            {
                Brand = "Samsung",
                Model = "Galaxy S24",
                PriceUSD = 899.00m,
                PurchaseDate = DateTime.Now.AddMonths(-6), // WHITE - 6 months old
                Office = swedenOffice
            };

            var computer2 = new Computer
            {
                Brand = "HP",
                Model = "EliteDesk 800",
                PriceUSD = 1100.00m,
                PurchaseDate = DateTime.Now.AddYears(-2).AddMonths(-7), // YELLOW
                Office = swedenOffice
            };

            var laptop3 = new Laptop
            {
                Brand = "Asus",
                Model = "ZenBook Pro",
                PriceUSD = 1599.00m,
                PurchaseDate = DateTime.Now.AddYears(-3).AddDays(45), // RED - very close to expiry
                Office = swedenOffice
            };

            // Add all assets
            assetManager.AddAsset(laptop1);
            assetManager.AddAsset(phone1);
            assetManager.AddAsset(computer1);
            assetManager.AddAsset(laptop2);
            assetManager.AddAsset(phone2);
            assetManager.AddAsset(computer2);
            assetManager.AddAsset(laptop3);

            Console.WriteLine($"\nSample data loaded: {assetManager.GetAssetCount()} assets added.");
        }
    }
}
