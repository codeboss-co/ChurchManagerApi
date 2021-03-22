namespace ChurchManager.Api
{
    public static class ApiRoutes
    {
        public const string DefaultCorsPolicy = "CorsPolicy";

        public static class HealthChecks
        {
            public const string DefaultUrl = "/api/health";
        }

        public static class Hubs
        {
            public const string NotificationHub = "/notifyhub";
        }
    }
}
