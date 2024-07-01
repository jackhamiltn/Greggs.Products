using Greggs.Products.Api.Controllers;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.DataTransferObjects;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Greggs.Products.UnitTests.Controllers
{
    [TestFixture]
    internal class ProductControllerTests
    {
        private Mock<IDataAccess<Product>> _dataAccessMock;
        private IProductService _productService;
        private ProductController _productController;
        private ICurrencyConverterService _currencyConverterService;
        private ILogger<ProductController> _logger;

        [SetUp]
        public void SetUp()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Information);
            });

            _logger = loggerFactory.CreateLogger<ProductController>();
            _currencyConverterService = new CurrencyConverterService();
            _dataAccessMock = new Mock<IDataAccess<Product>>();
            _dataAccessMock.Setup(d => d.GetMenu(It.IsAny<int>(), It.IsAny<int>())).Returns(TestData.Products);
            _productService = new ProductService(_dataAccessMock.Object, _currencyConverterService);
            _productController = new ProductController(_logger, _productService);
        }


        [TearDown]
        public void TearDown()
        {
            _dataAccessMock = null;
            _productService = null;
            _productController = null;
        }

        [Test]
        public void ProductsController_GetMenu_ReturnsMenu()
        {
            var result = _productController.GetMenu(0, 2) as ObjectResult;

            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var menu = result.Value as IEnumerable<Product>;

            Assert.Multiple(() =>
            {
                Assert.That(menu.Count, Is.EqualTo(2));
                Assert.That(menu.First().Name, Is.EqualTo("Sausage Roll"));
                Assert.That(menu.First().PriceInPounds, Is.EqualTo(1.00m));
                Assert.That(menu.Last().Name, Is.EqualTo("Vegan Sausage Roll"));
                Assert.That(menu.Last().PriceInPounds, Is.EqualTo(1.10m));
            });
        }

        [Test]
        public void ProductsController_GetMenuPageSizeGreaterThanMenuLength_ReturnsMenu()
        {
            var result = _productController.GetMenu(0, TestData.Products.Count + 1) as ObjectResult;
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var menu = result.Value as IEnumerable<Product>;

            Assert.That(menu.Count, Is.EqualTo(TestData.Products.Count));
        }

        [TestCase(0, -2)]
        [TestCase(-2, 2)]
        [TestCase(-2, -2)]
        public void ProductsController_GetMenuInvalidParameterData_Returns400BadRequest(int pageStart, int pageSize)
        {
            var result = _productController.GetMenu(pageStart, pageSize) as ObjectResult;

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(result.Value, Is.EqualTo("Invalid page size or page number."));
        }

        [Test]
        public void ProductsController_GetMenuInEuros_ReturnsMenuInEuros()
        {
            var result = _productController.GetMenuInEuros(0, 2) as ObjectResult;

            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var menu = result.Value as IEnumerable<ProductDataTransferObject>;

            Assert.Multiple(() =>
            {
                Assert.That(menu.Count, Is.EqualTo(2));
                Assert.That(menu.First().Name, Is.EqualTo("Sausage Roll"));
                Assert.That(menu.First().Price, Is.EqualTo(_currencyConverterService.ConvertGbpToEur(1.00m)));
                Assert.That(menu.Last().Name, Is.EqualTo("Vegan Sausage Roll"));
                Assert.That(menu.Last().Price, Is.EqualTo(_currencyConverterService.ConvertGbpToEur(1.10m)));

            });
        }

        [Test]
        public void ProductsController_GetMenuInEurosPageSizeGreaterThanMenuLength_ReturnsMenuInEuros()
        {
            var result = _productController.GetMenuInEuros(0, TestData.Products.Count + 1) as ObjectResult;

            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That(result.StatusCode, Is.EqualTo(200));

            var menu = result.Value as IEnumerable<ProductDataTransferObject>;

            Assert.That(menu.Count, Is.EqualTo(TestData.Products.Count));
        }

        [TestCase(0, -2)]
        [TestCase(-2, 2)]
        [TestCase(-2, -2)]
        public void ProductsController_GetMenuInEurosInvalidParameterData_Returns400BadRequest(int pageStart, int pageSize)
        {
            var result = _productController.GetMenuInEuros(pageStart, pageSize) as ObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
                Assert.That(result.Value, Is.EqualTo("Invalid page size or page number."));
            });
        }
    }
}
