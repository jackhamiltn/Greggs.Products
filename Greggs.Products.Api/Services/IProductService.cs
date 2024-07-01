using Greggs.Products.Api.DataTransferObjects;
using Greggs.Products.Api.Models;
using System.Collections.Generic;

namespace Greggs.Products.Api.Services
{
    public interface IProductService
    {
        public IEnumerable<Product> GetMenu(int? pageStart, int? pageSize);

        public IEnumerable<ProductDataTransferObject> GetMenuInEuros(int? pageStart, int? pageSize);
    }
}
