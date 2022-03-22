namespace ChurchManager.Domain.Common.Configuration
{
    public class BugsnagOptions : Bugsnag.Configuration
    {
        public bool Enabled { get; set; } = false;
    }
}
