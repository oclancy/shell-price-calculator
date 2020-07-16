using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace PriceCalculator.Extensions
{
    public static class StringCurrencyConversions
    {
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

        public static string ToCurrencyStringFromDecimal(this decimal source)
        {
           
            if (source<1)
            {
                // remove "£0." and add 'p'
                return $"{source:C2}p".Substring(3);
            }
            return $"{source:C2}";
        }
    }
}
