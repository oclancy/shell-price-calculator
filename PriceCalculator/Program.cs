using System;
using System.Linq;

namespace PriceCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter a items top add to the basket.");
                return;
            }

            Shelf shelf = new Shelf();
            Discounts discounts = new Discounts();

            Basket basket = new Basket(discounts.BasketPriceStrategies, discounts.ItemPriceStrategies);

            foreach (var arg in args)
            {
                var item = shelf.GetItem(arg);
                if(item != null)
                    basket.Add(item);
            }

            PrintSubTotal(basket);
            PrintDiscounts(basket);
        }


        static public void PrintSubTotal(Basket basket)
        {
            Console.WriteLine($"Subtotal: {basket.Sum(i => i.Price):C2}");
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
