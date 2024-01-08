using Bulky_crud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bulky_crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryContext _categoryContext;
        public CategoryController(CategoryContext categoryContext)
        {
            _categoryContext = categoryContext;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<Category>>> GetEmployee()
        {
            if (_categoryContext.Categories == null)
            {
                return NotFound();
            }
            return await _categoryContext.Categories.ToListAsync();
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            if (_categoryContext.Categories == null)
            {
                return NotFound();
            }
            var category = await _categoryContext.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return category;
        }

        [HttpPost]
        public async Task<ActionResult<Category>> PostEmployee(Category category)
        {
            _categoryContext.Categories.Add(category);
            await _categoryContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = category.ID }, category);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Category>> PutCategory(int id, Category category)
        {
            if (id != category.ID)
            {
                return BadRequest();
            }
            _categoryContext.Entry(category).State = EntityState.Modified;

            try
            {
                await _categoryContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok();
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteCategory(int id)
        {
            if (_categoryContext.Categories == null)
            {
                return NotFound();
            }

            var category = await _categoryContext.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            _categoryContext.Categories.Remove(category);
            await _categoryContext.SaveChangesAsync();

            return Ok();
        }
    }
}
