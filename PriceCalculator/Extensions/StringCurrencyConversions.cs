using System;
using System.Globalization;

namespace PriceCalculator.Extensions
{
    /// <summary>
    /// Provides naive string to decimal and decimal to string conversions to 
    /// fulfill the requirements of the spec.
    /// </summary>
    public static class StringCurrencyConversions
    {
        /// <summary>
        /// Converts to decimal from currency.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">source</exception>
        public static decimal ToDecimalFromCurrency(this string source)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentNullException(nameof(source));

            int factor = 1;
            if (source[source.Length-1] == 'p')
            {
                source = source.Substring(0, source.Length-1);
                // was the source just whole pence? If so divide by 100
                factor = source.Contains('.')? 1:100;
            }
            return decimal.Parse(source, NumberStyles.Any)/factor;
        }

        /// <summary>
        /// Converts decimal to Sterling currency string.
        /// Pretty naive implementation to fulfill brief
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static string ToCurrencyStringFromDecimal(this decimal source)
        {
           
            if (source<1)
            {
                // remove "£0." and add 'p'. Remove leading zeroes
                return $"{source:C2}p".Substring(3)
                                      .Trim('0');
            }
            return $"{source:C2}";
        }
    }
}
