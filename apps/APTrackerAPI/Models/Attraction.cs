using System.ComponentModel.DataAnnotations;

namespace APTrackerAPI.Models
{
    public class Attraction
    {
        /// <summary>
        /// Unique identifier for the attraction.
        /// </summary>
        public int AttractionId { get; set; }
        /// <summary>
        /// Name of the attraction.
        /// </summary>
        [Required]
        [StringLength(255)]
        public required string AttractionName { get; set; }
        /// <summary>
        /// Indicates whether waiting times should be tracked for this attraction.
        /// </summary>
        public bool TrackWaitingTime { get; set; } = true;
    }
}
