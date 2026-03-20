namespace ProductList
{
    public class Product
    {
        public string Category { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }

        public Product(string category, string name, decimal price)
        {
            Category = category;
            Name = name;
            Price = price;
        }

        public override string ToString()
        {
            return $"{Category,-15} {Name,-20} {Price,10:C}";
        }
    }
}