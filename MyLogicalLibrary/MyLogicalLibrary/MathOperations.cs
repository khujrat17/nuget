using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLogicalLibrary
{
    public class MathOperations
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Subtract(int a, int b)
        {
            return a - b;
        }

        public int Multiply(int a, int b)
        {
            return a * b;
        }

        public double Divide(int a, int b)
        {
            if (b == 0)
                throw new DivideByZeroException("Denominator cannot be zero.");
            return (double)a / b;
        }

        public bool IsPrime(int number)
        {
            if (number <= 1) return false;
            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0) return false;
            }
            return true;
        }

        public List<int> GenerateFibonacci(int count)
        {
            if (count <= 0) return new List<int>();
            if (count == 1) return new List<int> { 0 };
            if (count == 2) return new List<int> { 0, 1 };

            List<int> fibonacciSeries = new List<int> { 0, 1 };

            for (int i = 2; i < count; i++)
            {
                int nextNumber = fibonacciSeries[i - 1] + fibonacciSeries[i - 2];
                fibonacciSeries.Add(nextNumber);
            }

            return fibonacciSeries;
        }

        public long Factorial(int number)
        {
            if (number < 0)
                throw new ArgumentException("Factorial is not defined for negative numbers.");
            if (number == 0 || number == 1)
                return 1;

            long result = 1;
            for (int i = 2; i <= number; i++)
            {
                result *= i;
            }
            return result;
        }

        public string CheckEvenOrOdd(int number)
        {
            return number % 2 == 0 ? $"{number} is Even." : $"{number} is Odd.";
        }

        public string PrintOddNumbersInRange(int start, int end)
        {
            var oddNumbers = "Odd numbers in the range:";
            for (int i = start; i <= end; i++)
            {
                if (i % 2 != 0)
                    oddNumbers += $" {i}";
            }
            return oddNumbers;
        }

        public string CheckPositiveOrNot(int number)
        {
            return number > 0 ? $"{number} is Positive." : number < 0 ? $"{number} is Negative." : "The number is Zero.";
        }

        public string FindLargestOfTwoNumbers(int num1, int num2)
        {
            return $"The largest number is {(num1 > num2 ? num1 : num2)}.";
        }

        public void SwapTwoNumbers(ref int num1, ref int num2)
        {
            int temp = num1;
            num1 = num2;
            num2 = temp;
        }

        public string CheckDivisibleByTwo(int number)
        {
            return number % 2 == 0 ? $"{number} is divisible by 2." : $"{number} is not divisible by 2.";
        }

        public int FindSumOfMultiplesOf3And5(int limit)
        {
            int sum = 0;
            for (int i = 1; i < limit; i++)
            {
                if (i % 3 == 0 || i % 5 == 0)
                    sum += i;
            }
            return sum;
        }

        public string PrintMultiplesOf17(int limit)
        {
            var multiplesOf17 = "Multiples of 17 less than 100:";
            for (int i = 1; i < limit; i++)
            {
                if (i % 17 == 0)
                    multiplesOf17 += $" {i}";
            }
            return multiplesOf17;
        }

        public int FindSumOfDigits(int number)
        {
            int sum = 0;
            while (number != 0)
            {
                sum += number % 10;
                number /= 10;
            }
            return sum;
        }

        public int FindSumOfDigitsUsingRecursion(int number)
        {
            if (number == 0) return 0;
            return (number % 10) + FindSumOfDigitsUsingRecursion(number / 10);
        }

        public int ReverseNumber(int number)
        {
            int reversed = 0;
            while (number != 0)
            {
                reversed = reversed * 10 + number % 10;
                number /= 10;
            }
            return reversed;
        }

        public bool ReverseAndCheckPalindrome(int number)
        {
            int reversed = ReverseNumber(number);
            return number == reversed;
        }

        public string SumOfTwoBinaryNumbers(string binary1, string binary2)
        {
            int sum = Convert.ToInt32(binary1, 2) + Convert.ToInt32(binary2, 2);
            return Convert.ToString(sum, 2);
        }

        public string MultiplyTwoBinaryNumbers(string binary1, string binary2)
        {
            int product = Convert.ToInt32(binary1, 2) * Convert.ToInt32(binary2, 2);
            return Convert.ToString(product, 2);
        }

        public (int sum, int difference, int product, double quotient) CalculateBasicOperations(int a, int b)
        {
            return (a + b, a - b, a * b, (double)a / b);
        }

        public int MultiplyExponents(int baseNum, int exponent)
        {
            return (int)Math.Pow(baseNum, exponent);
        }

        public double DivideExponents(int baseNum, int exponent)
        {
            return Math.Pow(baseNum, 1.0 / exponent);
        }

        public string PrintBinaryEquivalent(int number)
        {
            return Convert.ToString(number, 2);
        }

        public string PrintMultiplicationTable(int number)
        {
            var multiplicationTable = $"Multiplication table for {number}:";
            for (int i = 1; i <= 10; i++)
            {
                multiplicationTable += $"\n{number} x {i} = {number * i}";
            }
            return multiplicationTable;
        }

        public string ReadGradeAndDisplayDescription(char grade)
        {
            return grade switch
            {
                'A' => "Excellent",
                'B' => "Good",
                'C' => "Average",
                'D' => "Below Average",
                'F' => "Fail",
                _ => "Invalid Grade"
            };
        }

        public string ConvertCase(string input)
        {
            return input.ToUpper() == input ? input.ToLower() : input.ToUpper();
        }

        public string CategorizeHeight(int height)
        {
            if (height < 150)
                return "Short";
            else if (height >= 150 && height <= 180)
                return "Average";
            else
                return "Tall";
        }
    }

    public class StringOperations
    {
        public string Reverse(string input)
        {
            char[] charArray = input.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public bool IsPalindrome(string input)
        {
            string reversed = Reverse(input);
            return string.Equals(input, reversed, StringComparison.OrdinalIgnoreCase);
        }

        public int CountAlphabets(string input)
        {
            return input.Count(char.IsLetter);
        }

        public Dictionary<char, int> CountDuplicates(string input)
        {
            var duplicates = new Dictionary<char, int>();
            foreach (char c in input)
            {
                if (char.IsLetterOrDigit(c))
                {
                    if (duplicates.ContainsKey(c))
                        duplicates[c]++;
                    else
                        duplicates[c] = 1;
                }
            }
            return duplicates.Where(x => x.Value > 1).ToDictionary(x => x.Key, x => x.Value);
        }

        public bool HasDuplicateCharacters(string input)
        {
            var charSet = new HashSet<char>();
            foreach (char c in input)
            {
                if (char.IsLetterOrDigit(c))
                {
                    if (!charSet.Add(c)) // If Add returns false, it means the character is a duplicate
                        return true;
                }
            }
            return false;
        }
    }

}
