using PriceCalculator.Extensions;

using System;

namespace PriceCalculator.PricingStrategies
{
    /// <summary>
    /// Describes a discount for a single target item type
    /// </summary>
    /// <seealso cref="PriceCalculator.PricingStrategies.IAmAnItemPricingStrategy" />
    public class ItemPercDiscountPriceStrategy : IAmAnItemPriceStrategy
    {
        private string TargetItemType { get; }
        private decimal PercDiscount { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemPercDiscount"/> class.
        /// </summary>
        /// <param name="itemtype">The itemtype.</param>
        /// <param name="percDiscount">The perc discount.</param>
        /// <exception cref="ArgumentException">Should be less than 100 - percDiscount</exception>
        /// <exception cref="ArgumentException">itemtype</exception>
        public ItemPercDiscountPriceStrategy(string itemtype, decimal percDiscount)
        {
            if (percDiscount > 100 || percDiscount <= 0) throw new ArgumentException("Should be between 0 and 100", nameof(percDiscount));

            if (string.IsNullOrEmpty(itemtype))
                throw new ArgumentException(nameof(itemtype));

            TargetItemType = itemtype;
            PercDiscount = percDiscount;
        }

        /// <summary>
        /// Gets the discount.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public (decimal, string) GetDiscount(Item item)
        {
            if (item.Name == TargetItemType)
            {
                var discount = item.Price * (PercDiscount / 100);
                return (discount, $"{TargetItemType} {PercDiscount}% off: -{discount.ToCurrencyStringFromDecimal()}");
            }

            return (0m, string.Empty);
        }
    }
}
