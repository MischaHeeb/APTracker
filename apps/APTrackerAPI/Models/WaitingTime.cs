namespace APTrackerAPI.Models
{
    public class WaitingTime
    {
        /// <summary>
        /// Unique identifier for this waiting time.
        /// </summary>
        public int WaitingTimeId { get; set; }
        /// <summary>
        /// Unique identifier for attraction this waiting time is referring to.
        /// </summary>
        public required int AttractionId { get; set; }
        /// <summary>
        /// The timestamp when this waiting time was recorded.
        /// </summary>
        public required DateTime Timestamp { get; set; }
        /// <summary>
        /// Waiting time in minutes.
        /// </summary>
        public required int WaitingTimeInMinutes { get; set; }

        /// <summary>
        /// The attraction this waiting time belongs to.
        /// </summary>
        public Attraction? Attraction { get; set; }
    }
}
