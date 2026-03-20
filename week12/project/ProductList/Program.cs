// See https://aka.ms/new-console-template for more information
using ProductList;

bool exit = true;
ProductManager productList = new ProductManager();

while (exit)
{
  Console.ForegroundColor = ConsoleColor.DarkYellow;

  Console.WriteLine("Enter a Number");
  Console.WriteLine("1-Add a Product");
  Console.WriteLine("2-Search a Product");
  Console.WriteLine("3-List Products");
  Console.WriteLine("0-Quit");

  Console.ResetColor();

  string? input = Console.ReadLine();

  switch (input)
  {
    case "1":
      Console.Write("Enter Categoria: ");
      string? category = Console.ReadLine();

      if (string.IsNullOrWhiteSpace(category))
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Error: Category cannot be empty.");
        Console.ResetColor();
        break;
      }

      Console.Write("Enter Name: ");
      string? name = Console.ReadLine();

      if (string.IsNullOrWhiteSpace(name))
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Error: Name cannot be empty.");
        Console.ResetColor();
        break;
      }

      Console.Write("Enter price: ");
      decimal price = 0;
      bool isValidPrice = false;

      while (!isValidPrice)
      {
        isValidPrice = decimal.TryParse(Console.ReadLine(), out price);
        if (!isValidPrice)
        {
          Console.Write("Invalid price. Please try again.: ");
        }
        else if (price <= 0)
        {
          Console.Write("The price must be greater than 0. Please try again.: ");
          isValidPrice = false;
        }
      }

      Product product = new Product(category, name, price);
      productList.AddProduct(product);

      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("The product was successfully added.\n");
      Console.ResetColor();

      break;
    case "2":
      Console.Write("Write the product name: ");
      string? searchTerm = Console.ReadLine();

      if (string.IsNullOrWhiteSpace(searchTerm))
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Error: The search term cannot be empty.");
        Console.ResetColor();
        break;
      }

      List<Product> searchResults = productList.SearchProducts(searchTerm);
      if (searchResults.Count > 0)
      {
        Console.WriteLine($"\n It found  {searchResults.Count} product(s):");
        productList.DisplayProducts(searchTerm);
      }
      else
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Product not found.");
        Console.ResetColor();
      }
      break;

    case "3":
      productList.DisplayProducts();
      break;
    case "0":
      exit = false;
      break;
    default:
      Console.WriteLine("The value was not found.");
      break;
  }
}
