using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ProductCrud.Database;

namespace ProductCrud.Functions
{
    public class DeleteProduct
    {
        private readonly ILogger<DeleteProduct> _logger;
        private readonly ShopContext _context;

        public DeleteProduct(ILogger<DeleteProduct> logger, ShopContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Function("DeleteProduct")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "DeleteProduct/{id}")] HttpRequest req, 
            string id)
        {
            _logger.LogInformation("Deleting Product");
            var product = await _context.Product.FindAsync(Guid.Parse(id));
            if (product == null) return new NotFoundResult();

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}
