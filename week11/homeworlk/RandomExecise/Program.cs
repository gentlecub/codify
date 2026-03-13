// See https://aka.ms/new-console-template for more information
using System.ComponentModel;
using RandomExecise;
/*
Random r = Random.Shared;

//Execise 1
Console.WriteLine("Enter a number");
string? input = Console.ReadLine();

if (int.TryParse(input, out int number))
{
  List<int> numberRandomGenerate = new List<int>();
  int sumTotal = 0;

  int i = 0;
  while (i <= number)
  {
    int numberRandon = r.Next(1, 100);
    numberRandomGenerate.Add(numberRandon);
    sumTotal += numberRandon;
    i++;
  }

  Console.WriteLine($"{numberRandomGenerate.Count - 1}: random numbers: {string.Join(' ', numberRandomGenerate)}");
  Console.WriteLine($"The sum is : {sumTotal}");
}


Console.WriteLine("\n--- Exercise 2: OddPositive ---");

int randomNumber = r.Next(-10, 11);

string parity = (randomNumber % 2 == 0) ? "even" : "odd";

string symbol;
if (randomNumber > 0)
  symbol = "positive";
else if (randomNumber < 0)
  symbol = "negative";
else
  symbol = "zero";

Console.WriteLine($"The generated number is: {randomNumber}");
Console.WriteLine($"{randomNumber} is odd and {symbol} ");


//Exercice 4

Console.Write("Enter number of integers to be generated ");
string? inputRamdom = Console.ReadLine();
int numb;
List<int> b = new List<int>();

if (int.TryParse(inputRamdom, out numb) && numb > 0)
{
  for (int i = 0; i <= numb; i++)
  {
    b.Add(r.Next(1, 100));
  }
}
else
{
  Console.WriteLine("The number have to be positive");
}
double average = b.Average();
int valueMax = b.Max();
int valueMin = b.Min();

Console.WriteLine($"Generated values:{string.Join(' ', b)}");
Console.WriteLine($"Average, min, and max are {average}, {valueMax}, and {valueMin}");*/
TwoDiceThat.TwoDice();