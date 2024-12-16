using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductCrud.Database;

namespace ProductCrud.Functions
{
    public class GetProducts
    {
        private readonly ILogger<GetProducts> _logger;
        private readonly ShopContext _context;

        public GetProducts(ILogger<GetProducts> logger, ShopContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Function("GetProducts")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            _logger.LogInformation("Getting products");
            var products = await _context.Product.ToListAsync();
            return new OkObjectResult(products);
        }
    }
}
