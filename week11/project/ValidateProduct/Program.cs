// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

Console.WriteLine("Enter products.Finish by typing 'exit'");

List<string> validProducts = new List<string>();

while (true)
{

  Console.Write("Ange product: ");
  string? input = Console.ReadLine();

  if (string.IsNullOrEmpty(input))
  {
    Console.WriteLine("You cannot enter an empty value.");
    continue;
  }

  if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
  {
    break;
  }

  string[] part = input.Split('-');
  if (part.Length != 2)
  {
    Console.WriteLine("Incorrect format. Use: ADK-123");
    continue;
  }

  if (Regex.IsMatch(part[0], @"^[A-Za-z]+$") && int.TryParse(part[1], out int number) && (number >= 200 && number <= 500))
  {
    validProducts.Add(input);
  }
  else if (!Regex.IsMatch(part[0], @"^[A-Za-z]+$"))
  {
    Console.WriteLine("Incorrect format on the left part of the product number");
    continue;
  }
  else if (int.TryParse(part[1], out number) && (number < 200 || number > 500))
  {
    Console.WriteLine("The numeric part must be between 200 and 500");
  }
  else
  {
    Console.WriteLine("Incorrect format on the right part of the product number");
    continue;
  }

}

Console.WriteLine("You specified the following products(sorted):");
validProducts.Sort();

foreach (var item in validProducts)
{
  Console.WriteLine(item);
}
Console.ReadKey();