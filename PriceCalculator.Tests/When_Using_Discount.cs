using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace PriceCalculator.Tests
{
    [TestClass]
    public class When_Using_Discount
    {
        [TestMethod]
        public void Can_Load_Discounts()
        {
            var discounts = new Discounts();

            Assert.AreEqual(1, discounts.ItemPriceStrategies.Count());

            Assert.AreEqual(1, discounts.BasketPriceStrategies.Count());
        }

        [TestMethod]
        public void Will_Not_Throw_If_File_Not_Found()
        {
            var discounts = new Discounts("notreal.txt");

            Assert.AreEqual(0, discounts.ItemPriceStrategies.Count());

            Assert.AreEqual(0, discounts.BasketPriceStrategies.Count());
        }
    }
}
