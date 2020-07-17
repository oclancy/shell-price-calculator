using PriceCalculator.PricingStrategies;

using System.Collections.Generic;
using System.Linq;

namespace PriceCalculator
{
    /// <summary>
    /// Basket of items to calculte price for
    /// </summary>
    /// <seealso cref="System.Collections.Generic.List{PriceCalculator.Item}" />
    public class Basket : List<Item>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Basket"/> class.
        /// </summary>
        /// <param name="basketPricingStrategies">The basket pricing strategies.</param>
        /// <param name="itemPricingStrategies">The item pricing strategies.</param>
        public Basket(IEnumerable<IAmABasketPriceStrategy> basketPricingStrategies,
                      IEnumerable<IAmAnItemPriceStrategy> itemPricingStrategies)
        {
            BasketPricingStrategies = basketPricingStrategies ?? Enumerable.Empty<IAmABasketPriceStrategy>() ;
            ItemPricingStrategies = itemPricingStrategies ?? Enumerable.Empty<IAmAnItemPriceStrategy>();
        }

        private IEnumerable<IAmABasketPriceStrategy> BasketPricingStrategies { get; }
        private IEnumerable<IAmAnItemPriceStrategy> ItemPricingStrategies { get; }

        /// <summary>
        /// Gets the discount messages.
        /// </summary>
        /// <value>
        /// The discount messages.
        /// </value>
        public IEnumerable<string> DiscountMessages => _discountMessages;
        private IList<string> _discountMessages = new List<string>();

        /// <summary>
        /// Gets the sub total.
        /// </summary>
        /// <value>
        /// The sub total.
        /// </value>
        public decimal SubTotal { get; private set; }
        /// <summary>
        /// Gets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public decimal Price { get; private set; }

        /// <summary>
        /// Prices the basket.
        /// </summary>
        public void PriceBasket()
        {
            _discountMessages.Clear();

            SubTotal = this.Sum(i => i.Price);
            Price = SubTotal;

            //item strategies
            foreach (var strategy in ItemPricingStrategies)
            {
                foreach (var item in this)
                {
                    var (discount, message) = strategy.GetDiscount(item);
                    Price -= discount;
                    if(!string.IsNullOrEmpty(message))
                        _discountMessages.Add(message);
                }
            }

            //basket strategies
            foreach (var strategy in BasketPricingStrategies)
            {
                var (discount, message) = strategy.GetDiscount(this);
                Price -= discount;
                if (!string.IsNullOrEmpty(message))
                    _discountMessages.Add(message);
            }
        }

    }
}
