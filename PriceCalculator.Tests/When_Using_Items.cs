using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace PriceCalculator.Tests
{
    [TestClass]
    public class When_Using_Items
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Will_Thow_On_Empty_Name()
        {
            new Item(string.Empty, "80p", Unit.bag);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Will_Thow_On_Empty_Price()
        {
            new Item("Test", "", Unit.bag);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Will_Thow_On_Null_Price()
        {
            new Item("Test", null, Unit.bag);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Will_Thow_On_Zero_Price_Pence()
        {
            new Item("Test", "0p", Unit.bag);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Will_Thow_On_Zero_Price_GBP()
        {
            new Item("Test", "£0.00", Unit.bag);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Will_Thow_On_Negative_Price()
        {
            new Item("Test", "-8p", Unit.bag);
        }


        [TestMethod]
        public void Can_Create_Pence()
        {
            var test = new Item("Test", "80p", Unit.bag);

            Assert.AreEqual("Test", test.Name);
            Assert.AreEqual(Unit.bag, test.Unit);
            Assert.AreEqual(0.8m, test.Price);
        }
    }
}
