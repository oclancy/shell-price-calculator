using PriceCalculator.Extensions;
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
        /// <param name="sourceQty">The source qty required to invoke the discount.</param>
        /// <param name="targetItemType">Type of the target item.</param>
        /// <param name="percDiscount">The discount.</param>
        /// <exception cref="ArgumentNullException">sourceItemType</exception>
        public MultiBuyDiscountPriceStrategy(string sourceItemType, decimal sourceQty, string targetItemType, decimal percDiscount)
        {
            if (percDiscount > 100 || percDiscount <= 0) throw new ArgumentException("Should be between 0 ad 100", nameof(percDiscount));

            if (string.IsNullOrEmpty(sourceItemType))
                throw new ArgumentException(nameof(sourceItemType));

            SourceItemType = sourceItemType;

            TargetItemType = targetItemType ?? SourceItemType;

            // This is cast from decimal due to the naive implemnation of constructing the 
            // strategy using Acivator from file params
            Quantity = (int)sourceQty;

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
            
            // Activate discount
            if (qtyOfSourceItems >= Quantity)
            {
                //anything to discount?
                var qtyOfTargetType = basket.Count(i => i.Name == TargetItemType);
                if (qtyOfTargetType >= 1)
                {
                    var sourceType = basket.First(i => i.Name == SourceItemType);
                    var targetType = basket.First(i => i.Name == TargetItemType);
                    var eligibleDiscounts = qtyOfSourceItems / Quantity;
                    var discount = eligibleDiscounts * (targetType.Price / 100 * PercDiscount);
                    return (discount, $"MultiBuyDiscount applied {eligibleDiscounts} times for {qtyOfSourceItems} {SourceItemType}. " +
                                      $"{PercDiscount}% off {eligibleDiscounts} {targetType.Unit} of {TargetItemType}: -{discount.ToCurrencyStringFromDecimal()}");
                }
            }

            return (0m, string.Empty);

        }

    }
}
