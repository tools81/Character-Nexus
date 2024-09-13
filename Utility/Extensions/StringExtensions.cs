using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Utility
{
    public static class StringExtensions
    {
        private static readonly Regex sWhitespace = new Regex(@"\s+");
        public static string ReplaceWhitespace(this string input, string replacement) => sWhitespace.Replace(input, replacement);

        public static string FormatAzureCompliance(this string template)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            return template.Replace(" ", "-").ToLower();
        }

        public static string SwapTextAroundPeriod(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            int periodIndex = input.IndexOf('.');
            if (periodIndex == -1)
            {
                throw new ArgumentException("Input does not contain a period.", nameof(input));
            }

            // Extract the parts before and after the period
            string beforePeriod = input.Substring(0, periodIndex);
            string afterPeriod = input.Substring(periodIndex + 1);

            // Swap and concatenate
            return $"{afterPeriod}.{beforePeriod}";
        }

        public static List<string> DivideStringIntoWordArray(this string input, int maxSubstringLength)
        {
            List<string> result = new List<string>();
            string[] words = input.Split(' ');
            string currentString = "";

            foreach (var word in words)
            {
                if ((currentString + word).Length > maxSubstringLength)
                {
                    result.Add(currentString.Trim());
                    currentString = "";
                }
                currentString += word + " ";
            }

            if (!string.IsNullOrEmpty(currentString))
            {
                result.Add(currentString.Trim());
            }

            return result;
        }
    }
}