using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockmngAPI.Data;
using StockmngAPI.Models;
using StockmngAPI.Utils;
using System.Data.Common;

namespace StockmngAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;

        public AuthController(DataContext context)
        {
            _context = context;
        }   

        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser(RegisterSchema req)
        {
            var existingUser = await _context.Users.Where(u => u.Username == req.Username).FirstOrDefaultAsync();
            if(existingUser != null)
            {
                return BadRequest("Username already in use");
            }

            User user = new User();

            user.Username = req.Username;
            user.Password = req.Password;
            user.Email = req.Email;
            user.CreatedAt = DateTime.Now;
            user.LastUpdatedAt = DateTime.Now;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            OrderCart cart = new OrderCart();
            cart.UserId = user.Id;
            cart.Items = new List<OrderItemType>();
            await _context.OrderCarts.AddAsync(cart);
            await _context.SaveChangesAsync();
            return Ok("Successfully registered user");
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> LoginUser(LoginSchema req)
        {
            var user = await _context.Users.Where(u => u.Username == req.Username).FirstOrDefaultAsync();

            if(user == null)
            {
                return BadRequest("Incorrect Credentials");
            }

            if(req.Password != user.Password)
            {
                return BadRequest("Incorrect Credentials");
            }

            return user;
        }
    }
}
