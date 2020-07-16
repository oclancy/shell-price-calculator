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

        private static readonly string _filename = "discounts.txt";

        enum ItemParts
        {
            Name,
            Price,
            Unit
        }

        public Shelf()
        {
            ReadItems();
        }

        private void ReadItems()
        {
            try
            {
                var discounts = File.ReadAllLines(_filename);

                Items = discounts.Where(line => !line.TrimStart().StartsWith("#"))
                                .Select(line => CreateItem(line))
                                .Where(item => item != null)
                                .ToDictionary(i => i.Name, i => i);

            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"{_filename} not found. No discounts to apply");
            }
        }

        internal Item GetItem(string item)
        {
            return Items[item];
        }

        Item CreateItem(string line)
        {
            try
            {
                var parts = line.Split(',');
                return new Item(parts[(int)ItemParts.Name],
                                decimal.Parse(parts[(int)ItemParts.Price], NumberStyles.Any),
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
