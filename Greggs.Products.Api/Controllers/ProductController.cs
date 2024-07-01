using Greggs.Products.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IProductService _productService;

    public ProductController(ILogger<ProductController> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    [HttpGet]
    public IActionResult GetMenu(int pageStart = 0, int pageSize = 5)
    {
        _logger.Log(LogLevel.Information, LoggingMessages.ReceivedGetRequestMessage, "products", pageStart, pageSize);
        if (pageStart < 0 || pageSize < 0)
        {
            _logger.Log(LogLevel.Error, LoggingMessages.ReturnBadRequestMessage, "products", pageStart, pageSize);
            return BadRequest("Invalid page size or page number.");
        }

        _logger.Log(LogLevel.Information, LoggingMessages.ReturnOkResultMessage);
        return Ok(_productService.GetMenu(pageStart, pageSize));
    }

    [HttpGet]
    [Route("Eur")]
    public IActionResult GetMenuInEuros(int pageStart = 0, int pageSize = 5)
    {
        _logger.Log(LogLevel.Information, LoggingMessages.ReceivedGetRequestMessage, "products/eur", pageStart, pageSize);
        if (pageStart < 0 || pageSize < 0)
        {
            _logger.Log(LogLevel.Error, LoggingMessages.ReturnBadRequestMessage, "products", pageStart, pageSize);
            return BadRequest("Invalid page size or page number.");
        }

        _logger.Log(LogLevel.Information, LoggingMessages.ReturnOkResultMessage);
        return Ok(_productService.GetMenuInEuros(pageStart, pageSize));
    }
}