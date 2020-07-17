namespace PriceCalculator.PricingStrategies
{
    /// <summary>
    /// Basket discount strategy
    /// </summary>
    public interface IAmABasketPriceStrategy : IAmAPriceStrategy
    {
        /// <summary>
        /// Gets the discount.
        /// </summary>
        /// <param name="basket">The basket.</param>
        /// <returns></returns>
        (decimal, string) GetDiscount(Basket basket);
    }
}
