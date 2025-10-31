using APTrackerAPI.Data;
using APTrackerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APTrackerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AttractionController : ControllerBase
    {
        private readonly ILogger<AttractionController> _logger;
        private readonly APTrackerDbContext _dbContext;
        
        public AttractionController(ILogger<AttractionController> logger, APTrackerDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        
        [HttpGet("GetAttraction")]
        public async Task<ActionResult<IEnumerable<Attraction>>> GetAttraction()
        {
            return await _dbContext.Attractions.ToListAsync();
        }
        
        [HttpGet("GetAttraction/{id}")]
        public async Task<ActionResult<Attraction>> GetAttraction(int id)
        {
            var attraction = await _dbContext.Attractions.FindAsync(id);

            if (attraction == null)
            {
                return NotFound();
            }

            return attraction;
        }
    
    }
}

