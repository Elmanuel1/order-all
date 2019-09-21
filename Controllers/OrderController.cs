using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwaggerApp.Data;
using SwaggerApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SwaggerApp.Exceptions;
using SwaggerApp.Service;
using SwaggerApp.vo;

namespace SwaggerApp.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly SampleContext _context;
        private readonly IOrderService _orderService;

        public OrderController(SampleContext context, IOrderService orderService)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Fruit>> GetOrders() {
            throw  new NotFoundException();
            //return _context.Fruits.ToList();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Fruit>> GetOrderById(int id)
        {
            var fruit = await _context.Fruits.FindAsync(id);
            
            if (fruit == null)
            {
                return NotFound();
            }

            return fruit;
        }

        [HttpPost]
        public async Task<ActionResult<Fruit>> CreateOrder(OrderRequest orderRequest)
        {
            OrderResponse orderResponse = await _orderService.CreateOrderAsync(orderRequest);
            return CreatedAtAction(nameof(CreateOrder), routeValues: new { id = orderRequest.UseDefaultLocation}, value: orderResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Fruit fruit)
        {
            if (id != fruit.Id)
            {
                return BadRequest();
            }

            _context.Entry(fruit).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var fruit = await _context.Fruits.FindAsync(id);

            if (fruit == null)
            {
                return NotFound();
            }

            _context.Fruits.Remove(fruit);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
