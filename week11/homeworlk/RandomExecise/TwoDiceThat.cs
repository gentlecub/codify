using System;

namespace RandomExecise;

static class TwoDiceThat
{
  public static void TwoDice()
  {
    Random r = Random.Shared;

    int[] frecuencies = new int[13];

    for (int i = 0; i < 1000; i++)
    {
      int dice1 = r.Next(1, 7);
      int dice2 = r.Next(1, 7);
      int sum = dice1 + dice2;
      frecuencies[sum]++;
    }

    Console.WriteLine("Frequency table (sum,count) for rolling two dices 10000 times:");
    for (int i = 2; i < frecuencies.Length; i++)
    {
      Console.WriteLine($"{i,2} - {frecuencies[i]}");
    }
  }

}
