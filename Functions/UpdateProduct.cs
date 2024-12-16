using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductCrud.Database;
using ProductCrud.Model;

namespace ProductCrud.Functions
{
    public class UpdateProduct
    {
        private readonly ILogger<UpdateProduct> _logger;
        private readonly ShopContext _context;

        public UpdateProduct(ILogger<UpdateProduct> logger, ShopContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Function("UpdateProduct")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "put")] HttpRequest req)
        {
            _logger.LogInformation("Updating Product");
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var product = JsonConvert.DeserializeObject<Product>(body);
            if (product == null) return new BadRequestResult();

            product.Name = product.Name.Trim().ToUpper();

            var oldProduct = await _context.Product.FindAsync(product.Id);
            if (oldProduct == null) return new NotFoundResult();

            oldProduct.Name = product.Name;
            oldProduct.Price = product.Price;

            await _context.SaveChangesAsync();

            return new OkObjectResult(product);
        }
    }
}
