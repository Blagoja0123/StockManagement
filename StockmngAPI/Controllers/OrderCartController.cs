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
    public class OrderCartController : ControllerBase
    {
        private readonly DataContext _context;

        public OrderCartController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderCart>>> GetAllCarts()
        {
            return await _context.OrderCarts.Include(c => c.Items).ToListAsync();
        }

        [HttpPut("addItem")]
        public async Task<IActionResult> AddItemToCart(OrderItemTypeSchema req)
        {
            var cart = await _context.OrderCarts.Where(c => c.UserId == req.UserId).Include(c => c.Items).FirstOrDefaultAsync();
            if(cart.Items == null)
            {
                cart.Items = new List<OrderItemType>();
            }
            OrderItemType item = new OrderItemType();
            item.UserId = req.UserId;
            item.ProductId = req.ProductId;
            item.ProductName = req.ProductName;
            item.Quantity = req.Quantity;
            item.Price = req.Price;
            item.TotalPrice = req.Price * req.Quantity;
            await _context.OrderItemTypes.AddAsync(item);
            await _context.SaveChangesAsync();
            cart.Items.Add(item);
            _context.Entry(cart).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok("Successfully added item to cart");
        }

        [HttpPut("{id}/removeItem")]
        public async Task<IActionResult> RemoveItemFromCart(int id, int itemId)
        {
            var cart = await _context.OrderCarts.Where(c => c.Id == id).Include(c => c.Items).FirstOrDefaultAsync();
            var item = await _context.OrderItemTypes.Where(i => i.Id == itemId).FirstOrDefaultAsync();
            if(item == null)
            {
                return NotFound("Item not found");
            }
            cart.Items.Remove(item);
            _context.Entry(cart).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok("Successfully removed item"); 
        }
    }
}
