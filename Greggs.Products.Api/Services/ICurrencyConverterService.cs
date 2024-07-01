namespace Greggs.Products.Api.Services
{
    public interface ICurrencyConverterService
    {
        public decimal ConvertGbpToEur(decimal gbp);
    }
}
