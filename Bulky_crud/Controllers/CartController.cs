using Bulky_crud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bulky_crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CategoryContext _cartService;

        public CartController(CategoryContext cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCart()
        {
            if (_cartService.CartDetails == null)
            {
                return NotFound();
            }
            return await _cartService.CartDetails.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(int id)
        {
            if (_cartService.CartDetails == null)
            {
                return NotFound();
            }

            var cart = await _cartService.CartDetails.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            return cart;
        }

        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
            _cartService.CartDetails.Add(cart);
            await _cartService.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCart), new { id = cart.ID }, cart);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Cart>> PutCart(int id, Cart cart)
        {
            if (id != cart.ID)
            {
                return BadRequest();
            }

            var existingCart = await _cartService.CartDetails.FindAsync(id);

            if (existingCart == null)
            {
                return NotFound();
            }

            // Update the properties of existingCart with the values from the incoming cart
            existingCart.ProductID = cart.ProductID;
            existingCart.Count = cart.Count;
            existingCart.UserId = cart.UserId;

            try
            {
                await _cartService.SaveChangesAsync();
                return Ok(existingCart); // Return the modified cart
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCart(int id)
        {
            if (_cartService.CartDetails == null)
            {
                return NotFound();
            }

            var cart = await _cartService.CartDetails.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            _cartService.CartDetails.Remove(cart);
            await _cartService.SaveChangesAsync();

            return Ok();
        }
    }
}
