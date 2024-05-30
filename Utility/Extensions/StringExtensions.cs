using System;

namespace Utility 
{
    public static class StringExtensions
    {
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
                throw new ArgumentException("Input cannot be null or empty.", nameof(input));
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
    }
}