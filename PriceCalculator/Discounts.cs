using PriceCalculator.PricingStrategies;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PriceCalculator
{
    /// <summary>
    /// Loads discounts from file
    /// Default file is ".\discounts.txt"
    /// All discounts must exist in the executing assembly
    /// Discount load failure is non-terminating.
    /// </summary>
    public class Discounts
    {

        /// <summary>
        /// The line parts
        /// </summary>
        enum StrategyParts
        {
            DiscountStrategyType,
            StrategyParameters
        }

        private IList<IAmAPriceStrategy> AllPriceStrategies = new List<IAmAPriceStrategy>();

        public IEnumerable<IAmAnItemPriceStrategy> ItemPriceStrategies => AllPriceStrategies.Where(s => s is IAmAnItemPriceStrategy)
                                                                                            .Cast<IAmAnItemPriceStrategy>();
        public IEnumerable<IAmABasketPriceStrategy> BasketPriceStrategies => AllPriceStrategies.Where(s => s is IAmABasketPriceStrategy)
                                                                                               .Cast<IAmABasketPriceStrategy>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Discounts"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public Discounts(string source = "discounts.txt")
        {
            try
            {
                var discounts = File.ReadAllLines(source);

                AllPriceStrategies = discounts.Where(line => !line.TrimStart().StartsWith("#"))
                                              .Select(line => CreatePriceStrategy(line))
                                              .Where(strategy => strategy != null)
                                              .ToList();

            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"{source} not found. No discounts to apply");
            }
        }

        /// <summary>
        /// Creates the price strategy.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <returns></returns>
        IAmAPriceStrategy CreatePriceStrategy(string line)
        {
            var parts = line.Split(',');

            // naive parameter parsing
            object[] strategyParameters = parts[(int)StrategyParts.StrategyParameters]
                                                                  .Split('|')
                                                                  .Select( (s) => {
                                                                      if (decimal.TryParse(s, out var dec))
                                                                          return dec as object;
                                                                      else
                                                                          return s as object;
                                                                  })
                                                                  .ToArray();

            try
            {
                // dynamically create
                var strategy = Activator.CreateInstance(Assembly.GetExecutingAssembly().FullName,
                                                        parts[(int)StrategyParts.DiscountStrategyType],
                                                        false,
                                                        0,
                                                        null,
                                                        strategyParameters,
                                                        null,
                                                        null);
                return strategy.Unwrap() as IAmAPriceStrategy;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not create discount strategy with parameters: line {line}");
                Console.WriteLine($"{ex.Message}");
            }

            return null;
        }
    }
}
