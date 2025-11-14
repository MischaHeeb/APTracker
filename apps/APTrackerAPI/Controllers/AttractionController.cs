using APTrackerAPI.Data;
using APTrackerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APTrackerAPI.Controllers
{
    /// <summary>
    /// Controller for managing attractions.
    /// </summary>
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

        /// <summary>
        /// Retrieves all attractions.
        /// </summary>
        /// <returns>A list of all attractions.</returns>
        /// <response code="200">Returns the list of attractions</response>
        [HttpGet("GetAttraction")]
        public async Task<ActionResult<IEnumerable<Attraction>>> GetAttraction()
        {
            return await _dbContext.Attractions.ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific attraction by a given ID.
        /// </summary>
        /// <param name="id">The ID of the attraction to retrieve.</param>
        /// <returns>The attraction specified by the ID.</returns>
        /// <response code="200">Returns the requested attraction</response>
        /// <response code="404">If the attraction was not found</response>
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

