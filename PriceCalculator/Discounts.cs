using PriceCalculator.PricingStrategies;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PriceCalculator
{
    public class Discounts
    {
        enum StrategyParts
        {
            ItemType,
            DiscountStrategyType,
            StrategyParameters
        }

        private IList<IAmAPriceStrategy> AllPriceStrategies = new List<IAmAPriceStrategy>();

        public IEnumerable<IAmAnItemPriceStrategy> ItemPriceStrategies => AllPriceStrategies.Where(s => s is IAmAnItemPriceStrategy)
                                                                                            .Cast<IAmAnItemPriceStrategy>();
        public IEnumerable<IAmABasketPriceStrategy> BasketPriceStrategies => AllPriceStrategies.Where(s => s is IAmABasketPriceStrategy)
                                                                                               .Cast<IAmABasketPriceStrategy>();

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

        IAmAPriceStrategy CreatePriceStrategy(string line)
        {
            // line format is 
            //DiscountStrategy,StrategyParameters
            var parts = line.Split();

            // naive parameter parsing
            object[] strategyParameters = parts[(int)StrategyParts.StrategyParameters]
                                                                  .Split(' ')
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
                                                        strategyParameters);
                return strategy as IAmAPriceStrategy;
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
