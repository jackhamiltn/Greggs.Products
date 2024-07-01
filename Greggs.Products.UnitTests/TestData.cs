using System.Collections.Generic;
using Greggs.Products.Api.Models;

namespace Greggs.Products.UnitTests
{
    internal static class TestData
    {
        internal static readonly IList<Product> Products = new List<Product>
        {
            new() { Name = "Sausage Roll", PriceInPounds = 1.00m },
            new() { Name = "Vegan Sausage Roll", PriceInPounds = 1.10m }
        };
    }
}
