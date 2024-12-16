using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductCrud.Database;
using ProductCrud.Model;

namespace ProductCrud.Functions
{
    public class CreateProduct
    {
        private readonly ILogger<CreateProduct> _logger;
        private readonly ShopContext _context;

        public CreateProduct(ILogger<CreateProduct> logger, ShopContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Function("CreateProduct")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("Creating Product");
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var product = JsonConvert.DeserializeObject<Product>(body);
            if (product == null) return new BadRequestResult();

            product.Name = product.Name.Trim().ToUpper();

            await _context.Product.AddAsync(product);
            await _context.SaveChangesAsync();

            return new CreatedResult($"GetProduct/{product.Id}", product.Id);
        }
    }
}
