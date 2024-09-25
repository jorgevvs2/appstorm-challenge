using CrudAppStorm.app.Domain.Entities;
using CrudAppStorm.app.Repositories;
using CrudAppStorm.app.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity.Infrastructure;

namespace CrudAppStorm.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductRepository _productRepository;

        public ProductsController(IProductRepository repository) {
            _productRepository = repository;
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<Product>> GetProductById(int productId)
        {
            if (ProductExists(productId).Result)
            {
                return await _productRepository.GetById(productId);
            }
            return NotFound();
        }

        [HttpGet]
        public Task<IEnumerable<Product>> GetAllProducts()
        {   
            return _productRepository.GetAll();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            await _productRepository.Update(product);

            try
            {
                await _productRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromBody] Product product)
        {
            await _productRepository.Insert(product);
            await _productRepository.Save();

            return CreatedAtAction(nameof(PostProduct), new { id = product.Id }, product);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productRepository.Delete(id);
            await _productRepository.Save();

            return product;
        }

        private async Task<bool> ProductExists(int id)
        {
            return await _productRepository.Exists(id);
        }
    }
}
