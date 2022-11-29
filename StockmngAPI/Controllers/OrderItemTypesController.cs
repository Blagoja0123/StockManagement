using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockmngAPI.Data;
using StockmngAPI.Models;

namespace StockmngAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemTypesController : ControllerBase
    {
        private readonly DataContext _context;

        public OrderItemTypesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/OrderItemTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItemType>>> GetOrderItemTypes()
        {
            return await _context.OrderItemTypes.ToListAsync();
        }

        // GET: api/OrderItemTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItemType>> GetOrderItemType(int id)
        {
            var orderItemType = await _context.OrderItemTypes.FindAsync(id);

            if (orderItemType == null)
            {
                return NotFound();
            }

            return orderItemType;
        }

        // PUT: api/OrderItemTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderItemType(int id, OrderItemType orderItemType)
        {
            if (id != orderItemType.Id)
            {
                return BadRequest();
            }

            _context.Entry(orderItemType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemTypeExists(id))
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

        // POST: api/OrderItemTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderItemType>> PostOrderItemType(OrderItemType orderItemType)
        {
            _context.OrderItemTypes.Add(orderItemType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderItemType", new { id = orderItemType.Id }, orderItemType);
        }

        // DELETE: api/OrderItemTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItemType(int id)
        {
            var orderItemType = await _context.OrderItemTypes.FindAsync(id);
            if (orderItemType == null)
            {
                return NotFound();
            }

            _context.OrderItemTypes.Remove(orderItemType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderItemTypeExists(int id)
        {
            return _context.OrderItemTypes.Any(e => e.Id == id);
        }
    }
}
