using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockmngAPI.Data;
using StockmngAPI.Models;

namespace StockmngAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("cart/{id}")]
        public async Task<ActionResult<OrderCart>> GetUserCart(int id)
        {
            var cart = await _context.OrderCarts.Where(c => c.UserId == id).FirstOrDefaultAsync();
            return Ok(cart);
        }

        [HttpGet("orders/{id}")]
        public async Task<ActionResult<List<Order>>> GetUserCarts(int id)
        {
            var orders = await _context.Orders.Where(o => o.UserId == id).ToListAsync();
            if(orders == null)
            {
                return NotFound("No orders found");
            }

            return Ok(orders);
        }
    }
}
