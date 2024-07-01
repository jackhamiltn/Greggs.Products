using System;

namespace Greggs.Products.Api.Services
{
    public class CurrencyConverterService : ICurrencyConverterService
    {
        public const decimal GbpToEurExchangeRate = 1.11m;
        private const int numOfDecimals = 2;

        public decimal ConvertGbpToEur(decimal gbp)
        {
            if (gbp < 0 || gbp > decimal.MaxValue / GbpToEurExchangeRate)
            {
                throw new ArgumentOutOfRangeException(nameof(gbp), CurrencyConverterExceptions.OutOfRangeExceptionMessage);
            }

            // Round down to two places.
            return Math.Round(gbp * GbpToEurExchangeRate, numOfDecimals, MidpointRounding.ToNegativeInfinity);
        }
    }
}
