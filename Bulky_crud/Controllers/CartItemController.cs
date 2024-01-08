using Bulky_crud.Migrations;
using Bulky_crud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bulky_crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly CategoryContext _cartItem;

        public CartItemController(CategoryContext cartItem)
        {
            _cartItem = cartItem;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<CartItem>>> GetItem()
        {
            if (_cartItem.CartItems == null)
            {
                return NotFound();
            }
            return await _cartItem.CartItems.ToListAsync();
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<CartItem>> GetItem(int id)
        {
            if (_cartItem.CartItems == null)
            {
                return NotFound();
            }
            var cart = await _cartItem.CartItems.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            return cart;
        }

        [HttpPost]
        public async Task<ActionResult<CartItem>> PostItem(CartItem cartItem)
        {
            _cartItem.CartItems.Add(cartItem);
            await _cartItem.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItem), new { id = cartItem.Id }, cartItem);
        }

        //[HttpPut("{id}")]
        //public async Task<ActionResult<CartItem>> PutItem(int id, CartItem cartItem)
        //{
        //    if (id != cartItem.Id)
        //    {
        //        return BadRequest();
        //    }
        //    _cartItem.Entry(cartItem).State = EntityState.Modified;

        //    try
        //    {
        //        await _cartItem.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        throw;
        //    }
        //    return Ok();
        //}


        [HttpPut("{id}")]
        public async Task<ActionResult<CartItem>> PutItem(int id, CartItem cartItem)
        {
            if (id != cartItem.Id)
            {
                return BadRequest();
            }

            var existingCartItem = await _cartItem.CartItems.FindAsync(id);
            if (existingCartItem == null)
            {
                return NotFound();
            }

            existingCartItem.Quantity = cartItem.Quantity; // Update quantity

            try
            {
                await _cartItem.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(existingCartItem);
        }



        //[HttpPut("{id}")]
        //public async Task<ActionResult<CartItem>> PutItem(int id, CartItem cartItem)
        //{
        //    if (id != cartItem.Id)
        //    {
        //        return BadRequest();
        //    }

        //    var existingCartItem = await _cartItem.CartItems.FindAsync(id);
        //    if (existingCartItem == null)
        //    {
        //        return NotFound();
        //    }

        //    existingCartItem.Quantity = cartItem.Quantity; // Update quantity

        //    try
        //    {
        //        await _cartItem.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        throw;
        //    }
        //    return Ok();
        //}






        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteItem(int id)
        {
            if (_cartItem.CartItems == null)
            {
                return NotFound();
            }

            var cartItem = await _cartItem.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }
            _cartItem.CartItems.Remove(cartItem);
            await _cartItem.SaveChangesAsync();

            return Ok();
        }
        private bool CartItemExists(int id)
        {
            return _cartItem.CartItems.Any(e => e.Id == id);
        }

    }
}

