using System;
using System.Linq;

namespace PriceCalculator.PricingStrategies
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="PriceCalculator.PricingStrategies.IAmABasketPriceStrategy" />
    public sealed class MultiBuyDiscountPriceStrategy : IAmABasketPriceStrategy
    {
        string SourceItemType { get; }
        string TargetItemType { get; }

        int Quantity { get; }
        decimal PercDiscount { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiBuyDiscountPriceStrategy"/> class.
        /// </summary>
        /// <param name="sourceItemType">Type of the source item.</param>
        /// <param name="sourceQty">The source qty.</param>
        /// <param name="targetItemType">Type of the target item.</param>
        /// <param name="percDiscount">The discount.</param>
        /// <exception cref="ArgumentNullException">sourceItemType</exception>
        public MultiBuyDiscountPriceStrategy(string sourceItemType, int sourceQty, string targetItemType, decimal percDiscount)
        {
            if (percDiscount > 100 || percDiscount < 0) throw new ArgumentException("Should be between 0 ad 100", nameof(percDiscount));

            if (string.IsNullOrEmpty(sourceItemType))
                throw new ArgumentException(nameof(sourceItemType));

            SourceItemType = sourceItemType;

            TargetItemType = targetItemType ?? SourceItemType;

            Quantity = sourceQty;

            PercDiscount = percDiscount;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiBuyDiscountPriceStrategy"/> class.
        /// </summary>
        /// <param name="sourceItemType">Type of the source item.</param>
        /// <param name="sourceQty">The source qty.</param>
        /// <param name="percDiscount">The perc discount.</param>
        public MultiBuyDiscountPriceStrategy(string sourceItemType, int sourceQty, decimal percDiscount)
            :this(sourceItemType,sourceQty, sourceItemType, percDiscount)
        {
        }

        /// <summary>
        /// Gets the discount.
        /// </summary>
        /// <param name="basket">The basket.</param>
        /// <returns></returns>
        public (decimal, string) GetDiscount(Basket basket)
        {
            var qtyOfSourceItems = basket.Count(i => i.Name == SourceItemType);
            
            if (qtyOfSourceItems >= Quantity)
            {
                var type = basket.First(i => i.Name == SourceItemType);
                var qtyOfTargetItems = basket.Count(i => i.Name == TargetItemType);
                var eligibleDiscounts = qtyOfSourceItems / Quantity;
                var discount = eligibleDiscounts * type.Price * PercDiscount;
                return (discount, $"MultiBuyDiscount applied {eligibleDiscounts} times for {qtyOfSourceItems} {SourceItemType}. " +
                                  $"{PercDiscount}% off {eligibleDiscounts} {type.Unit} of {TargetItemType}: -{discount:C2}");
            }

            return (0m, string.Empty);

        }

    }
}
