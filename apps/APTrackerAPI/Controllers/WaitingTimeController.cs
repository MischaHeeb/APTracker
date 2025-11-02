using APTrackerAPI.Data;
using APTrackerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APTrackerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WaitingTimeController : ControllerBase
    {
        private readonly ILogger<WaitingTimeController> _logger;
        private readonly APTrackerDbContext _dbContext;

        public WaitingTimeController(ILogger<WaitingTimeController> logger, APTrackerDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet("GetDailyTrend/{attractionId}")]
        public async Task<ActionResult<IEnumerable<WaitingTime>>> GetDailyTrend(int attractionId, [FromQuery] DateTime? date)
        {
            var targetDate = date?.Date ?? DateTime.UtcNow.Date;

            var startDate = targetDate;

            var endDate = targetDate.AddDays(1);
            
            var dailyData = await _dbContext.WaitingTime
                .Where(elem => 
                    elem.AttractionId == attractionId
                    && elem.Timestamp >= startDate && elem.Timestamp < endDate)
                .OrderBy(elem => elem.Timestamp)
                .ToListAsync();
            
            return Ok(dailyData);
        }
        
        [HttpGet("GetYearlyTrend/{attractionId}")]
        public async Task<ActionResult<IEnumerable<WaitingTime>>> GetYearlyTrend(int attractionId)
        {
            var startDate = DateTime.UtcNow.AddYears(-1);

            var yearlyData = await _dbContext.WaitingTime
                .Where(elem =>
                    elem.AttractionId == attractionId
                    && elem.Timestamp >= startDate)
                .GroupBy(elem => elem.Timestamp.Date)
                .Select(group => new DailyAvgWaitingTime
                {
                    Date = group.Key,
                    AvgWaitingTime = group.Average(time => time.WaitingTimeInMinutes)
                })
                .OrderBy(avg => avg.Date)
                .ToListAsync();
            
            return Ok(yearlyData);
        }
    }
}

