using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ProductCrud.Database;

namespace ProductCrud.Functions
{
    public class GetProduct
    {
        private readonly ILogger<GetProduct> _logger;
        private readonly ShopContext _context;

        public GetProduct(ILogger<GetProduct> logger, ShopContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Function("GetProduct")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetProduct/{id}")] HttpRequest req, string id)
        {
            _logger.LogInformation("Getting a single Product");
            var product = await _context.Product.FindAsync(Guid.Parse(id));
            if (product == null) return new NotFoundResult();

            return new OkObjectResult(product);
        }
    }
}
