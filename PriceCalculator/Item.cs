using PriceCalculator.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PriceCalculator
{
    /// <summary>
    /// Represents an item that can be purchased
    /// </summary>
    public class Item
    {
        /// <summary>
        /// The unit
        /// </summary>
        public Unit Unit;

        public Item(string name, string price, Unit unit)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            Price = price.ToDecimalFromCurrency();
            if (Price <= 0)
                throw new ArgumentException("Price must be positive value", nameof(price));
            Unit = unit;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public decimal Price { get; }
    }
}
