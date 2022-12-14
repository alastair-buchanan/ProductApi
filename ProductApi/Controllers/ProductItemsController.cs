using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.Models;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductItemsController : ControllerBase
    {
        private readonly ProductContext _context;
        private readonly ILogger<ProductItemsController> _logger;

        public ProductItemsController(ProductContext context, ILogger<ProductItemsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/ProductItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductItem>>> GetProductItems()
        {
            if (_context.ProductItems == null)
            {
                return NotFound();
            }

            return await _context.ProductItems.ToListAsync();
        }

        // GET: api/ProductItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductItem>> GetProductItem(long id)
        {
            if (_context.ProductItems == null)
            {
                return NotFound();
            }

            var productItem = await _context.ProductItems.FindAsync(id);

            if (productItem == null)
            {
                return NotFound();
            }

            return productItem;
        }

        // PUT: api/ProductItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductItem(long id, ProductItem productItem)
        {
            if (id != productItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(productItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductItemExists(id))
                {
                    _logger.LogInformation("Product item {Name} does not exist.", productItem.Name);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProductItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductItem>> PostProductItem(ProductItem productItem)
        {
            if (_context.ProductItems == null)
            {
                return Problem("Entity set 'ProductContext.ProductItems'  is null.");
            }

            _context.ProductItems.Add(productItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductItem), new {id = productItem.Id}, productItem);
        }

        // DELETE: api/ProductItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductItem(long id)
        {
            if (_context.ProductItems == null)
            {
                return NotFound();
            }

            var productItem = await _context.ProductItems.FindAsync(id);
            if (productItem == null)
            {
                return NotFound();
            }

            _context.ProductItems.Remove(productItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductItemExists(long id)
        {
            return (_context.ProductItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}