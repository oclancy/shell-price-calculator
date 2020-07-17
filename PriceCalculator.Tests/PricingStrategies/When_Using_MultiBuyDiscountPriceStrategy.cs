using Microsoft.VisualStudio.TestTools.UnitTesting;
using PriceCalculator.PricingStrategies;
using System;
using System.Linq;

namespace PriceCalculator.PriceStrategies.Tests
{
    [TestClass]
    public class When_Using_MultiBuyDiscountPriceStrategy
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Will_Throw_On_Discount_0()
        {
            new MultiBuyDiscountPriceStrategy("Apples",2,0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Will_Throw_On_Discount_Greater_Than_100()
        {
            new MultiBuyDiscountPriceStrategy("Apples", 2, 101);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Will_Throw_On_Source_Item_Empty()
        {
            new MultiBuyDiscountPriceStrategy("", 2, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Will_Throw_On_Source_Item_Null()
        {
            new MultiBuyDiscountPriceStrategy(null, 2, 0);
        }

        [TestMethod]
        public void Will_Discount_Matched_Item_Source_Apples_Target_Apples()
        {
            // discount 10% on 2 apples
            var strategy = new MultiBuyDiscountPriceStrategy("Apples", 2, 10);

            Item testItem = new Item("Apples", "30p", Unit.bag);
            var basket = new Basket(new []{ strategy }, Enumerable.Empty<IAmAnItemPriceStrategy>());
            
            //5 apples should be 2* discount = .30*5 - (2*(.30/100*10)) = 6p
            basket.Add(testItem);
            basket.Add(testItem);
            basket.Add(testItem);
            basket.Add(testItem);
            basket.Add(testItem);

            var (discount, message) = strategy.GetDiscount(basket);

            Assert.AreEqual(0.06m, discount);
            Assert.AreEqual($"MultiBuyDiscount applied 2 times for 5 Apples. 10% off 2 bag of Apples: -6p", message);
        }

        [TestMethod]
        public void Will_Not_Discount_Matched_Item_Source_Apples_Target_Apples_If_Qty_Not_Reached()
        {
            // discount 10% on 2 apples
            var strategy = new MultiBuyDiscountPriceStrategy("Apples", 2, 10);

            Item testItem = new Item("Apples", "30p", Unit.bag);
            var basket = new Basket(new[] { strategy }, Enumerable.Empty<IAmAnItemPriceStrategy>());

            //5 apples should be 2* discount = .30*5 - (2*(.30/100*10)) = 6p
            basket.Add(testItem);

            var (discount, message) = strategy.GetDiscount(basket);

            Assert.AreEqual(0.00m, discount);
            Assert.AreEqual(string.Empty, message);
        }


        [TestMethod]
        public void Will_Discount_Matched_Item_Source_Apples_Target_Bread()
        {
            // discount 10% on 2 apples
            var strategy = new MultiBuyDiscountPriceStrategy("Apples", 2, "Bread", 10);

            Item testApple = new Item("Apples", "30p", Unit.bag);
            Item testLoaf = new Item("Bread", "£1.20p", Unit.loaf);
            var basket = new Basket(new[] { strategy }, Enumerable.Empty<IAmAnItemPriceStrategy>());

            //5 apples should be 2* discount = .30*5 - (2*(.30/100*10)) = 6p
            basket.Add(testApple);
            basket.Add(testApple);
            basket.Add(testLoaf);

            var (discount, message) = strategy.GetDiscount(basket);

            Assert.AreEqual(0.12m, discount);
            Assert.AreEqual($"MultiBuyDiscount applied 1 times for 2 Apples. 10% off 1 loaf of Bread: -12p", message);
        }
    }
}
