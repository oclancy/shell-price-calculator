using Microsoft.VisualStudio.TestTools.UnitTesting;
using PriceCalculator.PricingStrategies;
using System;

namespace PriceCalculator.PriceStrategies.Tests
{
    [TestClass]
    public class When_Using_ItemDiscountPriceStrategy
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Will_Throw_On_Discount_0()
        {
            new ItemPercDiscountPriceStrategy("Apples", 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Will_Throw_On_Discont_Greater_Than_100()
        {
            new ItemPercDiscountPriceStrategy("Apples", 101);
        }


        [TestMethod]
        public void Will_Discount_Matched_Item()
        {
            var strategy = new ItemPercDiscountPriceStrategy("Apples", 10);

            Item testItem = new Item("Apples", "30p", Unit.bag);

            var (discount,message) = strategy.GetDiscount(testItem);

            Assert.AreEqual(0.03m, discount);
            Assert.AreEqual("Apples 10% off: -3p", message);
        }

        [TestMethod]
        public void Will_Not_Discount_Unmatched_Item()
        {
            var strategy = new ItemPercDiscountPriceStrategy("Apples", 10);

            Item testItem = new Item("Bread", "10p", Unit.bag);

            var (discount, message) = strategy.GetDiscount(testItem);

            Assert.AreEqual(0m, discount);
            Assert.AreEqual(string.Empty, message);
        }
    }
}
