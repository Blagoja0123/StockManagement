using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockmngAPI.Data;
using StockmngAPI.Models;
using StockmngAPI.Utils;

namespace StockmngAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return Ok(product);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddProduct(ProductSchema req)
        {
            Product product = new Product();
            product.Name = req.Name;
            product.Description = req.Description;
            product.Price = req.Price;
            product.Quantity = req.Quantity;
            product.CreatedAt = DateTime.Now;
            product.UpdatedAt = DateTime.Now;

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return Ok("Successfully added product");

        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if(id != product.Id)
            {
                return BadRequest("An error occured");
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok("Product updated successfully");
            }catch (DbUpdateConcurrencyException)
            {
                return BadRequest("An error occured while saving the changes");
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product == null)
            {
                return NotFound("Product not found");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok("Successfully deleted item");
        }
    }
}
