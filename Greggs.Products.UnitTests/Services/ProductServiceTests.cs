using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace Greggs.Products.UnitTests.Services
{
    [TestFixture]
    internal class ProductServiceTests
    {
        private Mock<IDataAccess<Product>> _dataAccessMock;
        private IProductService _productService;
        private ICurrencyConverterService _currencyConverterService;

        [SetUp]
        public void SetUp()
        {
            _currencyConverterService = new CurrencyConverterService();
            _dataAccessMock = new Mock<IDataAccess<Product>>();
            _dataAccessMock.Setup(d => d.GetMenu(It.IsAny<int>(), It.IsAny<int>())).Returns(TestData.Products);
            _productService = new ProductService(_dataAccessMock.Object, _currencyConverterService);
        }


        [TearDown]
        public void TearDown()
        {
            _dataAccessMock = null;
            _productService = null;
        }

        [Test]
        public void ProductService_GetMenu_ReturnsMenu()
        {
            var result = _productService.GetMenu(0, 2);

            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(2));
                Assert.That(result.First().Name, Is.EqualTo("Sausage Roll"));
                Assert.That(result.First().PriceInPounds, Is.EqualTo(1.00m));
                Assert.That(result.Last().Name, Is.EqualTo("Vegan Sausage Roll"));
                Assert.That(result.Last().PriceInPounds, Is.EqualTo(1.10m));
            });
        }

        [Test]
        public void ProductService_GetMenuInEuros_ReturnsMenuInEuros()
        {
            var result = _productService.GetMenuInEuros(0, 2);

            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(2));
                Assert.That(result.First().Name, Is.EqualTo("Sausage Roll"));
                Assert.That(result.First().Price, Is.EqualTo(_currencyConverterService.ConvertGbpToEur(1.00m)));
                Assert.That(result.Last().Name, Is.EqualTo("Vegan Sausage Roll"));
                Assert.That(result.Last().Price, Is.EqualTo(_currencyConverterService.ConvertGbpToEur(1.10m)));
            });
        }
    }
}
