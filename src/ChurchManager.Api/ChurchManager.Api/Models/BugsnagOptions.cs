namespace ChurchManager.Api.Models
{
    public class BugsnagOptions : Bugsnag.Configuration
    {
        public bool Enabled { get; set; } = false;
    }
}
