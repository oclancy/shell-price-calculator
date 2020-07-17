
namespace PriceCalculator.PricingStrategies
{
    /// <summary>
    /// Item discount behaviour
    /// </summary>
    public interface IAmAnItemPriceStrategy : IAmAPriceStrategy
    {
        /// <summary>
        /// Gets the discount.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The dsicount and a message for output</returns>
        (decimal, string) GetDiscount(Item item);
    }
}
