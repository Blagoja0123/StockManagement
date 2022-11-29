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
    public class OrderController : ControllerBase
    {
        private readonly DataContext _context;

        public OrderController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetAllOrders()
        {
            return await _context.Orders.Include(o => o.Cart).ToListAsync();
        }
        
        [HttpPost("add")]
        public async Task<IActionResult> AddOrder(OrderSchema req)
        {
            var cart = await _context.OrderCarts.Where(c => c.Id == req.OrderCartId).Include(c => c.Items).FirstOrDefaultAsync();
            if(cart?.Items == null)
            {
                return BadRequest("Cart is empty");
            }

            foreach(OrderItemType item in cart.Items)
            {
                var product = await _context.Products.Where(p => p.Id == item.ProductId).FirstOrDefaultAsync();
                product.Quantity -= item.Quantity;
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            Order order = new Order();
            order.OrderCartId = req.OrderCartId;
            order.Cart = cart.Items;
            order.Recipiant = req.Recipiant;
            order.CreatedAt = DateTime.Now;
            order.UpdatedAt = DateTime.Now;
            order.Status = Status.Pending;
            order.UserId = req.UserId;
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return Ok("Successfully placed order");
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateOrder(string status, int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if(order == null)
            {
                return NotFound("Order not found");
            }

            if(status == "Processing")
            {
                order.Status = Status.Processing;
                order.UpdatedAt = DateTime.Now;
                _context.Entry(order).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Updated order status to processing");
            }
            else if(status == "Completed")
            {
                order.Status = Status.Completed;
                order.UpdatedAt = DateTime.Now;
                _context.Entry(order).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Updated order status to completed");
            }
            else
            {
                order.Status = Status.Pending;
                order.UpdatedAt = DateTime.Now;
                _context.Entry(order).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Updated order status to pending");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.Where(o => o.Id == id).Include(o => o.Cart).FirstOrDefaultAsync();
            if(order == null)
            {
                return NotFound("Order not found");
            }

            foreach(OrderItemType item in order.Cart)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                product.Quantity += item.Quantity;
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return Ok("Successfully deleted order");
        }
    }
}
