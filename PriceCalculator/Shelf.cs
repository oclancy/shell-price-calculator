using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace PriceCalculator
{
    public class Shelf
    {
        public Dictionary<string, Item> Items { get; private set; }

        enum ItemParts
        {
            Name,
            Price,
            Unit
        }

        public Shelf(string source = "items.txt")
        {
            ReadItems(source);
        }

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
                Console.WriteLine($"{source} not found. No discounts to apply");
            }
        }

        internal Item GetItem(string item)
        {
            if (Items.ContainsKey(item))
                return Items[item];
            return null;
        }

        Item CreateItem(string line)
        {
            try
            {
                var parts = line.Split(',');
                return new Item(parts[(int)ItemParts.Name],
                                parts[(int)ItemParts.Price],
                                Enum.Parse<Unit>(parts[(int)ItemParts.Unit]));

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
