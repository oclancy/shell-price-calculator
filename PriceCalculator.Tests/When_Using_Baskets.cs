using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PriceCalculator.PricingStrategies;
using System.Linq;

namespace PriceCalculator.Tests
{
    [TestClass]
    public class When_Using_Baskets
    {
        private Mock<IAmABasketPriceStrategy> mockBasketStrategy;
        private Mock<IAmAnItemPriceStrategy> mockItemStrategy;
        Item testItem = new Item("Apples", "30p", Unit.bag);
        Item testItem2 = new Item("Bread", "50p", Unit.loaf);

        [TestInitialize]
        public void Initialize() 
        {
            mockBasketStrategy = new Mock<IAmABasketPriceStrategy>();
            mockBasketStrategy.Setup(m => m.GetDiscount(It.IsAny<Basket>())).Returns((10, "mock basket strategy"));

            mockItemStrategy = new Mock<IAmAnItemPriceStrategy>();
            mockItemStrategy.Setup(m => m.GetDiscount(It.IsAny<Item>())).Returns((20, "mock item strategy"));
        }
        
        [TestMethod]
        public void Can_Price_Basket_Using_Item_Strategy()
        {
            var basket = new Basket( Enumerable.Empty<IAmABasketPriceStrategy>(), new[] { mockItemStrategy.Object });

            basket.Add(testItem);

            basket.PriceBasket();

            Assert.AreEqual(30m, basket.SubTotal);
            Assert.AreEqual(10m, basket.Price);
            Assert.AreEqual(1, basket.DiscountMessages.Count());

            mockBasketStrategy.Verify(m => m.GetDiscount(It.IsAny<Basket>()), Times.Never);
            mockItemStrategy.Verify(m => m.GetDiscount(It.IsAny<Item>()), Times.Once);
        }

        [TestMethod]
        public void Can_Price_Basket_Using_Basket_Strategy()
        {
            var basket = new Basket(new[] { mockBasketStrategy.Object }, Enumerable.Empty<IAmAnItemPriceStrategy>());

            basket.Add(testItem2);

            basket.PriceBasket();

            Assert.AreEqual(50m, basket.SubTotal);
            Assert.AreEqual(40m, basket.Price);
            Assert.AreEqual(1, basket.DiscountMessages.Count());

            mockBasketStrategy.Verify(m => m.GetDiscount(It.IsAny<Basket>()), Times.Once);
            mockItemStrategy.Verify(m => m.GetDiscount(It.IsAny<Item>()), Times.Never);
        }

        [TestMethod]
        public void Can_Price_Basket_Using_Item_And_Basket_Strategy()
        {
            var basket = new Basket(new[] { mockBasketStrategy.Object }, new[] { mockItemStrategy.Object });
            
            basket.Add(testItem);
            basket.Add(testItem2);

            basket.PriceBasket();

            Assert.AreEqual(80m, basket.SubTotal);
            Assert.AreEqual(30m, basket.Price);
            Assert.AreEqual(3, basket.DiscountMessages.Count());

            mockBasketStrategy.Verify(m => m.GetDiscount(It.IsAny<Basket>()), Times.Once);
            mockItemStrategy.Verify(m => m.GetDiscount(It.IsAny<Item>()), Times.Exactly(2));
        }
    }
}
