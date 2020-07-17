using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace PriceCalculator.Tests
{
    [TestClass]
    public class When_Using_Shelf
    {
        [TestMethod]
        public void Can_Load_Items()
        {
            var shelf = new Shelf();

            Assert.AreEqual(4, shelf.Items.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void Will_Not_Throw_If_Source_File_NotFound()
        {
            var shelf = new Shelf("notreal.txt");

            Assert.AreEqual(0, shelf.Items.Count());
        }
    }
}
