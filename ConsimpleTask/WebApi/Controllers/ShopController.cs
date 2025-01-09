using ConsimpleTask.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsimpleTask.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly ShopDbContext _context;

        public ShopController(ShopDbContext context)
        {
            _context = context;
        }

        [HttpGet("birthdays")]
        public async Task<IActionResult> GetBirthdays([FromQuery] int? month, [FromQuery] int? day)
        {
            if (!month.HasValue || !day.HasValue)
            {
                return BadRequest("Month and day must be specified.");
            }

            if (month < 1 || month > 12 || day < 1 || day > 31)
            {
                return BadRequest("Wrong month or day.");
            }

            var birthdays = await _context.Customers
                .Where(c => c.BirthDate.Month == month && c.BirthDate.Day == day)
                .Select(c => new { c.Id, c.FullName })
                .ToListAsync();

            return Ok(birthdays);
        }


        [HttpGet("recent-buyers")]
        public async Task<IActionResult> GetRecentBuyers([FromQuery] int? days)
        {
            if (!days.HasValue || days <= 0)
            {
                return BadRequest("The number of days must be specified.");
            }
            
            var sinceDate = DateTime.Now.AddDays((double)-days);
            var buyers = await _context.Purchases
                .Where(p => p.Date >= sinceDate)
                .Join(_context.Customers,
                      p => p.CustomerId,
                      c => c.Id,
                      (p, c) => new { p, c })
                .GroupBy(pc => pc.p.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    FullName = g.Select(x => x.c.FullName).FirstOrDefault() ?? "",
                    LastPurchaseDate = g.Max(pc => pc.p.Date)
                })
                .ToListAsync();

            return Ok(buyers);
        }

        [HttpGet("popular-categories")]
        public async Task<IActionResult> GetPopularCategories([FromQuery] int? customerId)
        {
            if (!customerId.HasValue || customerId <= 0)
            {
                return BadRequest("CustomerId must be specified and greater than zero.");
            }

            var categories = await _context.Purchases
                .Where(p => p.CustomerId == customerId)
                .SelectMany(p => p.PurchaseItemsId)
                .Join(_context.PurchaseItems, id => id, pi => pi.Id, (id, pi) => pi.ProductId)
                .Join(_context.Products, productId => productId, p => p.Id, (productId, p) => p.Category)
                .GroupBy(category => category)
                .Select(group => new
                {
                    Category = group.Key,
                    TotalQuantity = group.Count()
                })
                .ToListAsync();

            return Ok(categories);
        }

    }
}
