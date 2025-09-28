namespace WorkerService.Common
{ 
    public static class WorkerService
    {
        public const string ReminderForUnsubscribers = "ReminderForUnsubscribers";
        public const string UpdateUserSegments = "UpdateUserSegments";
        public const string ExecuteCampaigns = "ExecuteCampaigns";
    }

    public static class WorkerServiceResult
    {
        public const string Passed = "Passed";
        public const string Failed = "Failed";
        public const string Undefined = "Undefined";
    } 
}