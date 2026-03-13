using System;

namespace RandomExecise;

static class ShortName
{
  public static string GetShortName(string firstName, string lastName)
  {
    string fName = firstName.Substring(0, 1);
    string lName = lastName.Length > 4 ? lastName.Substring(0, 4) : lastName;

    return $"{fName}. {lName}";
  }
}
