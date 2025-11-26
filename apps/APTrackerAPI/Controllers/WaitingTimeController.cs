using APTrackerAPI.Data;
using APTrackerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APTrackerAPI.Controllers
{
    /// <summary>
    /// Controller for managing waiting times
    /// </summary>
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

        /// <summary>
        /// Retrieves the daily trend of an attraction.
        /// By default, this method returns the daily trend of the current day.
        /// However, if the optional date parameter is added, the method returns the daily trend of that day.
        /// </summary>
        /// <param name="attractionId">The specified attraction from which the waiting times are obtained.</param>
        /// <param name="date">Optional parameter if daily trend should be of a specific day.</param>
        /// <returns>The daily trend of the given attraction.</returns>
        /// <response code="200">Returns the requested daily trend.</response>
        [HttpGet("GetDailyTrend/{attractionId}")]
        public async Task<ActionResult<IEnumerable<WaitingTime>>> GetDailyTrend(int attractionId,
            [FromQuery] DateTime? date)
        {
            var targetDate = date?.Date ?? DateTime.UtcNow.Date;

            var startDate = DateTime.SpecifyKind(targetDate, DateTimeKind.Utc);
            var endDate = startDate.AddDays(1);

            var dailyData = await _dbContext.WaitingTime
                .Where(elem =>
                    elem.AttractionId == attractionId
                    && elem.Timestamp >= startDate && elem.Timestamp < endDate)
                .OrderBy(elem => elem.Timestamp)
                .ToListAsync();

            return Ok(dailyData);
        }

        /// <summary>
        /// Retrieves the daily trend of an attraction.
        /// By default, this method returns the daily trend of the current day.
        /// However, if the optional date parameter is added, the method returns the daily trend of that day.
        /// </summary>
        /// <param name="attractionId">The specified attraction from which the waiting times are obtained.</param>
        /// <param name="date"></param>
        /// <param name="startYear">Optional parameter if yearly trend should be of a specific year.</param>
        /// <returns>The daily trend of the given attraction.</returns>
        /// <response code="200">Returns the requested daily trend.</response>
        [HttpGet("GetYearlyTrend/{attractionId}")]
        public async Task<ActionResult<IEnumerable<WaitingTime>>> GetYearlyTrend(int attractionId,
            [FromQuery] int? startYear)
        {
            DateTime startDate;
            if (startYear.HasValue)
            {
                var dateString = $"{startYear}-01-01 00:00:00";
                if (DateTime.TryParse(dateString, out var date))
                {
                    startDate = date;
                }
                else
                {
                    _logger.LogError("The following Date could not be parsed: {DateString}", dateString);
                    return BadRequest();
                }
            }
            else
            {
                startDate = DateTime.UtcNow.AddYears(-1);
            }

            startDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
            var endDate = startDate.AddYears(1);

            var yearlyData = await _dbContext.WaitingTime
                .Where(elem =>
                    elem.AttractionId == attractionId
                    && elem.Timestamp >= startDate
                    && elem.Timestamp < endDate)
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