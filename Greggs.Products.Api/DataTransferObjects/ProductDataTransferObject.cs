using System.ComponentModel.DataAnnotations;

namespace Greggs.Products.Api.DataTransferObjects
{
    public class ProductDataTransferObject
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
