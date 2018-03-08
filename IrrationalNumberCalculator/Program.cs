using System;
using System.Text;
using Microsoft.SolverFoundation.Common;

namespace MathStuff
{
    class IrrationalNumberCalculatorProgram
    {
        static void Main(string[] args)
        {
            int digitsToCalculate;
            while (true)
            {
                Console.WriteLine();
                Console.Write("How many digits do you want to calculate for PI and e? ");
                string number = Console.ReadLine();
                if (int.TryParse(number, out digitsToCalculate) && digitsToCalculate > 0)
                {
                    break;
                }
                Console.WriteLine($"{number} does not appear to be a positive number");
            }

            Console.WriteLine();
            Console.WriteLine("***************************************************");
            Console.WriteLine($"Calculating PI to {digitsToCalculate} places...");
            Console.WriteLine("***************************************************");
            CalculatePi(digitsToCalculate);

            Console.WriteLine();
            Console.WriteLine("Press Enter to calculate e");
            Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("***************************************************");
            Console.WriteLine($"Calculating e to {digitsToCalculate} places...");
            Console.WriteLine("***************************************************");
            CalculateEulersNumberE(digitsToCalculate);

            Console.ReadLine();
        }

        private static void CalculatePi(int digitsToCalculate)
        {
            var bignumTotal = new Rational();
            int bignumStringSize = digitsToCalculate + 2;  // +2 for "3." at beginning of string so digitsToCalculate means number of decimal places.
            string prevString = "0";
            var sb = new StringBuilder(bignumStringSize);
            int prefixLen = 0;
            int k = 0;
            do
            {
                long denom1 = 8 * k + 1;
                long denom2 = 8 * k + 4;
                long denom3 = 8 * k + 5;
                long denom4 = 8 * k + 6;

                long numer1 = 4 * denom2 * denom3 * denom4;
                long numer2 = 2 * denom1 * denom3 * denom4;
                long numer3 = 1 * denom1 * denom2 * denom4;
                long numer4 = 1 * denom1 * denom2 * denom3;

                long numer = numer1 - numer2 - numer3 - numer4;
                long denom = denom1 * denom2 * denom3 * denom4;

                Rational bignumIterationValue = Rational.Get(numer, denom) * Rational.Get(1, BigInteger.Power(16, k));

                bignumTotal += bignumIterationValue;
                string currentStr = bignumTotal.ToFixedLengthString(bignumStringSize, sb);
                prefixLen = GetNumPrefixDigits(currentStr, prevString);

                Console.WriteLine($"k={k} pi = {currentStr} ({prefixLen} common)");

                prevString = currentStr;
                k++;
            } while (prefixLen < bignumStringSize);

            k--;
            Console.WriteLine();
            Console.WriteLine("***************************************************");
            Console.WriteLine($"To get {digitsToCalculate} we had to run to k={k}");
            Console.WriteLine("***************************************************");
        }

        private static void CalculateEulersNumberE(int digitsToCalculate)
        {
            var bignumTotal = new Rational();
            int bignumStringSize = digitsToCalculate + 2;  // +2 for "2." at beginning of string so digitsToCalculate means number of decimal places.
            string prevString = "0";
            var sb = new StringBuilder(bignumStringSize);
            int prefixLen = 0;
            int n = 0;
            BigInteger factorialDenominator = 1;
            do
            {
                if (n > 0)
                {
                    factorialDenominator *= n;
                }

                bignumTotal += Rational.Get(1, factorialDenominator);
                string currentStr = bignumTotal.ToFixedLengthString(bignumStringSize, sb);
                prefixLen = GetNumPrefixDigits(currentStr, prevString);

                Console.WriteLine($"n={n} e = {currentStr} ({prefixLen} common)");

                prevString = currentStr;
                n++;
            } while (prefixLen < bignumStringSize);

            n--;
            Console.WriteLine();
            Console.WriteLine("***************************************************");
            Console.WriteLine($"To get {digitsToCalculate} we had to run to n={n}");
            Console.WriteLine("***************************************************");
        }

        private static int GetNumPrefixDigits(string str1, string str2)
        {
            int i = 0;
            for (; i < str1.Length || i < str2.Length; i++)
            {
                char c1 = i >= str1.Length ? '0' : str1[i];
                char c2 = i >= str2.Length ? '0' : str2[i];

                if (c1 != c2)
                {
                    return i + 1;
                }
            }

            return i;
        }
    }

    internal static class RationalExtensions
    {
        public static string ToFixedLengthString(this Rational r, int len, StringBuilder sb)
        {
            sb.Length = 0;
            r.AppendDecimalString(sb, len);
            if (sb.Length < len)
            {
                // Zero-fill remainder.
                sb.Append('0', len - sb.Length);
            }
            return sb.ToString();
        }
    }
}
