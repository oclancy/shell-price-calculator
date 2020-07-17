using PriceCalculator.Extensions;
using System;
using System.Linq;

namespace PriceCalculator
{
    /// <summary>
    /// PriceCalculator
    /// 
    /// Usage:
    /// PriceCalculator item item item
    /// 
    /// Source files:
    /// By default Items are loaded from items.txt
    /// By default discount strategies are loaded from discounts.txt
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter items to add to the basket.");
                return;
            }

            // Load items
            Shelf shelf = new Shelf();
            // Load discounts
            Discounts discounts = new Discounts();

            // create basket of items
            Basket basket = new Basket(discounts.BasketPriceStrategies, discounts.ItemPriceStrategies);

            foreach (var arg in args)
            {
                var item = shelf.GetItem(arg);
                if(item != null)
                    basket.Add(item);
            }

            // Run pricing
            basket.PriceBasket();

            // Output
            PrintSubTotal(basket);
            PrintDiscounts(basket);
            PrintTotal(basket);
        }


        static public void PrintSubTotal(Basket basket)
        {
            Console.WriteLine($"Subtotal: {basket.Sum(i => i.Price).ToCurrencyStringFromDecimal()}");
        }

        static public void PrintTotal(Basket basket)
        {
            Console.WriteLine($"Total: {basket.Price.ToCurrencyStringFromDecimal()}");
        }

        static public void PrintDiscounts(Basket basket)
        {
            if (basket.DiscountMessages.Any())
            {
                foreach (var message in basket.DiscountMessages)
                    Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine("(No offers available)");
            }
        }

    }
}
