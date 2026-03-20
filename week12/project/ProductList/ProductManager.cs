namespace ProductList
{
    public class ProductManager
    {
        private List<Product> _products;

        public ProductManager()
        {
            _products = new List<Product>();
        }

        public void AddProduct(Product product)
        {
            _products.Add(product);
        }

        public List<Product> GetSortedProducts()
        {
            return _products.OrderBy(p => p.Name).ToList();
        }

        public decimal GetTotalPrice()
        {
            return _products.Sum(p => p.Price);
        }

        public List<Product> SearchProducts(string searchTerm)
        {
            return _products
                .Where(p => p.Name.Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase))
                .OrderBy(p => p.Price)
                .ToList();
        }

        public void DisplayProducts(string? highlightTerm = null)
        {
            var sortedProducts = GetSortedProducts();

            Console.WriteLine("\n{0,-15} {1,-20} {2,10}", "CATEGORY", "NAME", "PRICE");
            Console.WriteLine(new string('-', 50));

            foreach (var product in sortedProducts)
            {
                if (highlightTerm != null &&
                    product.Name.Contains(highlightTerm, StringComparison.OrdinalIgnoreCase))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(product);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(product);
                }
            }

            Console.WriteLine(new string('-', 50));
            Console.WriteLine($"\n{"TOTAL:",10} {GetTotalPrice(),10:C}");
            Console.WriteLine();
        }

        public int Count => _products.Count;
    }
}