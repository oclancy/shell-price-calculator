using Microsoft.VisualStudio.TestTools.UnitTesting;
using PriceCalculator.Extensions;
using PriceCalculator.PricingStrategy;
using System.Linq;

namespace PriceCalculator.PriceStrategies.Tests
{
    [TestClass]
    public class When_Using_StringCurrencyConverstions
    {
        [TestMethod]
        public void Can_Convert_Price_From_Pence()
        {
            var price = "80p".ToDecimalFromCurrency();

            Assert.AreEqual(0.80m, price);
        }

        [TestMethod]
        public void Can_Convert_Price_From_GBP()
        {
            var price = "£1.80".ToDecimalFromCurrency();

            Assert.AreEqual(1.80m, price);
        }

        [TestMethod]
        public void Can_Convert_Price_From_GBP_And_Pence()
        {
            var price = "£1.80p".ToDecimalFromCurrency();

            Assert.AreEqual(1.80m, price);
        }

        [TestMethod]
        public void Can_Convert_Price_To_GBP_And_Pence()
        {
            var price = 1.80m.ToCurrencyStringFromDecimal();

            Assert.AreEqual("£1.80", price);
        }

        [TestMethod]
        public void Can_Convert_Price_To_Pence()
        {
            var price = 0.80m.ToCurrencyStringFromDecimal();

            Assert.AreEqual("80p", price);
        }

        [TestMethod]
        public void Can_Convert_Price_To_GBP()
        {
            var price = 1m.ToCurrencyStringFromDecimal();

            Assert.AreEqual("£1.00", price);
        }
    }
}