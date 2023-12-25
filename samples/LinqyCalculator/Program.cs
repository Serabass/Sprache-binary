using System;
using Sprache;

namespace LinqyCalculator
{
    [Obsolete("Don't use this")]
    class Program
    {
        static void Main()
        {
            
        }

        static bool Prompt(out string value)
        {
            Console.Write("Enter a numeric expression, or 'q' to quit: ");
            var line = Console.ReadLine();
            if (line.ToLowerInvariant().Trim() == "q")
            {
                value = null;
                return false;
            }
            else
            {
                value = line;
                return true;
            }
        }
    }
}
