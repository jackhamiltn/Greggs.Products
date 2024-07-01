using System.Linq;
using Greggs.Products.Api.DataAccess;
using NUnit.Framework;

namespace Greggs.Products.UnitTests.DataAccess
{
    [TestFixture]
    internal class ProductAccessTests
    {
        private ProductAccess _productAccess;

        [SetUp]
        public void Setup()
        {
            _productAccess = new ProductAccess();
        }

        [Test]
        public void GetMenu_PageStartAndPageSizeAreNull_ReturnsAllProducts()
        {
            var result = _productAccess.GetMenu(null, null);

            Assert.That(result.Count(), Is.EqualTo(8));
        }

        [Test]
        public void GetMenu_PageSizeIsSpecified_ReturnsSpecifiedNumberOfProducts()
        {
            var result = _productAccess.GetMenu(null, 3);

            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public void GetMenu_PageStartAndPageSizeAreSpecified_ReturnsCorrectProducts()
        {
            var result = _productAccess.GetMenu(2, 3);

            var products = result.ToList();
            Assert.Multiple(() =>
            {
                Assert.That(products.Count, Is.EqualTo(3));
                Assert.That(products[0].Name, Is.EqualTo("Steak Bake"));
                Assert.That(products[1].Name, Is.EqualTo("Yum Yum"));
                Assert.That(products[2].Name, Is.EqualTo("Pink Jammie"));
            });
        }

        [Test]
        public void GetMenu_PageSizeIsGreaterThanTotalNumberOfProducts_ReturnsAllProducts()
        {
            var result = _productAccess.GetMenu(null, 20);

            Assert.That(result.Count(), Is.EqualTo(8));
        }

        [Test]
        public void GetMenu_PageStartIsSpecified_SkipsCorrectNumberOfProducts()
        {
            var result = _productAccess.GetMenu(5, null);

            var products = result.ToList();
            Assert.Multiple(() =>
            {
                Assert.That(products.Count, Is.EqualTo(3));
                Assert.That(products[0].Name, Is.EqualTo("Mexican Baguette"));
                Assert.That(products[1].Name, Is.EqualTo("Bacon Sandwich"));
                Assert.That(products[2].Name, Is.EqualTo("Coca Cola"));
            });
        }
    }
}
