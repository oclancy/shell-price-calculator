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
        private string v1;
        private decimal v2;

        public Item(string v1, decimal v2, Unit unit)
        {
            this.v1 = v1;
            this.v2 = v2;
            Unit = unit;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public decimal Price { get; }
    }
}
