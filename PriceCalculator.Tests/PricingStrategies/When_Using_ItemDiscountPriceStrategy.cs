using Microsoft.VisualStudio.TestTools.UnitTesting;
using PriceCalculator.PricingStrategy;

namespace PriceCalculator.PriceStrategies.Tests
{
    [TestClass]
    public class When_Using_ItemDiscountPriceStrategy
    {
        [TestMethod]
        public void Will_Discount_Matched_Item()
        {
            var strategy = new ItemPercDiscountPriceStrategy("Apples", 10);

            Item testItem = new Item("Apples", "30p", Unit.bag);

            var (discount,message) = strategy.GetDiscount(testItem);

            Assert.AreEqual(3m, discount);
            Assert.AreEqual("Apples 30% off: -3p", message);
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
