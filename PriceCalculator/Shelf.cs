using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace PriceCalculator
{
    /// <summary>
    /// Represnts the avilable items
    /// Items are read from a source file
    /// Default source file name is ".\items.txt"
    /// </summary>
    public class Shelf
    {
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public Dictionary<string, Item> Items { get; private set; }

        /// <summary>
        /// The line parts
        /// </summary>
        enum ItemParts
        {
            Name,
            Price,
            Unit
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Shelf"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public Shelf(string source = "items.txt")
        {
            ReadItems(source);
        }

        /// <summary>
        /// Reads the items.
        /// Will throw and exit program if no items available
        /// </summary>
        /// <param name="source">The source.</param>
        private void ReadItems(string source)
        {
            try
            {
                var discounts = File.ReadAllLines(source);

                Items = discounts.Where(line => !line.TrimStart().StartsWith("#"))
                                .Select(line => CreateItem(line))
                                .Where(item => item != null)
                                .ToDictionary(i => i.Name, i => i);

            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"{source} not found. No items available. Terminating...");
                throw;
            }
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public Item GetItem(string item)
        {
            if (Items.ContainsKey(item))
                return Items[item];

            Console.WriteLine($"Unknown item '{item}'.");
            return null;
        }

        /// <summary>
        /// Creates the item from the line in the source
        /// </summary>
        /// <param name="line">The line.</param>
        /// <returns></returns>
        Item CreateItem(string line)
        {
            try
            {
                var parts = line.Split(',');
                if (parts.Count() == 3)
                {
                    return new Item(parts[(int)ItemParts.Name],
                                    parts[(int)ItemParts.Price],
                                    Enum.Parse<Unit>(parts[(int)ItemParts.Unit]));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not create Item with parameters: line {line}");
                Console.WriteLine($"{ex.Message}");
            }

            return null;
        }
    }
}
