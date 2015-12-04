namespace AnyTrack.Infrastructure
{
    /// <summary>
    /// Static class for referencing sprint story status.
    /// </summary>
    public static class ServiceSprintStoryStatus
    {
        /// <summary>
        /// Not started. 
        /// </summary>
        public const string NotStarted = "NOT_STARTED";

        /// <summary>
        /// In progress.
        /// </summary>
        public const string InProgress = "IN_PROGRESS";

        /// <summary>
        /// Awaiting test.
        /// </summary>
        public const string AwaitingTest = "AWAITING_TEST";

        /// <summary>
        /// In test.
        /// </summary>
        public const string InTest = "IN_TEST";

        /// <summary>
        /// Done property.
        /// </summary>
        public const string Done = "DONE";
    }
}