using System;
using Greggs.Products.Api.Services;
using NUnit.Framework;

namespace Greggs.Products.UnitTests.Services
{
    [TestFixture]
    internal class CurrencyConverterServiceTests
    {
        private readonly CurrencyConverterService _currencyConverterService = new CurrencyConverterService();

        public static object[] ValidGbpAndExpectedEur = new[]
        {
            new object[] { 1m, 1.11m },
            new object[] { 2m, 2.22m },
            new object[] { 74.36m, 82.53m },
            new object[] { 384.49m, 426.78m },
            new object[] { 0m, 0m },
        };

        [TestCaseSource(nameof(ValidGbpAndExpectedEur))]
        public void CurrencyConverter_ConvertValidGbp_ConvertsToEur(decimal gbp, decimal expectedEur)
        {
            Assert.That(_currencyConverterService.ConvertGbpToEur(gbp), Is.EqualTo(expectedEur));
        }

        public static object[] OutOfRangeNegativeGbp = new[]
        {
            new object[] { -0.01m},
            new object[] { -9m},
            new object[] { decimal.MinValue },
        };

        [TestCaseSource(nameof(OutOfRangeNegativeGbp))]
        public void CurrencyConverter_ConvertNegativeGbp_ThrowsArgumentOutOfRangeException(decimal gbp)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _currencyConverterService.ConvertGbpToEur(gbp));
        }

        public static object[] OutOfRangePositiveGbp = new[]
        {
            new object[] { 78,228,162,514,264,337,593,543,950,335m},
            new object[] { decimal.MaxValue / CurrencyConverterService.GbpToEurExchangeRate + 0.01m},
            new object[] { decimal.MaxValue },
        };

        [TestCaseSource(nameof(OutOfRangeNegativeGbp))]
        public void CurrencyConverter_ConvertOutOfRangeGbp_ThrowsArgumentOutOfRangeException(decimal gbp)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _currencyConverterService.ConvertGbpToEur(gbp));
        }
    }
}
