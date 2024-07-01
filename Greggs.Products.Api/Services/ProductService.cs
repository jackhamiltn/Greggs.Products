using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.DataTransferObjects;
using Greggs.Products.Api.Models;
using System.Collections.Generic;
using System.Linq;

namespace Greggs.Products.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IDataAccess<Product> _productAccess;
        private readonly ICurrencyConverterService _currencyConverterService;

        public ProductService(IDataAccess<Product> productAccess, ICurrencyConverterService currencyConverterService)
        {
            _productAccess = productAccess;
            _currencyConverterService = currencyConverterService;
        }

        public IEnumerable<Product> GetMenu(int? pageStart, int? pageSize)
        {
            return _productAccess.GetMenu(pageStart, pageSize);
        }

        public IEnumerable<ProductDataTransferObject> GetMenuInEuros(int? pageStart, int? pageSize)
        {
            var products = _productAccess.GetMenu(pageStart, pageSize)
                .Select(product => new ProductDataTransferObject
                {
                    Name = product.Name,
                    Price = _currencyConverterService.ConvertGbpToEur(product.PriceInPounds),
                });

            return products;
        }
    }
}
