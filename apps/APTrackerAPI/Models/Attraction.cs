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
        public required string AttractionName { get; set; }
        /// <summary>
        /// Indicates whether waiting times should be tracked for this attraction.
        /// </summary>
        public required bool TrackWaitingTime { get; set; } = true;

    }
}
