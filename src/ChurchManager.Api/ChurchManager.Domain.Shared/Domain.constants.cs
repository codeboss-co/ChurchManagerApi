namespace ChurchManager.Domain.Shared
{
    public static class DomainConstants
    {
        public static class Groups
        {
            public const int NoParentGroupId = 0;
        }

        public static class Discipleship
        {
            public static class StepStatus
            {

                public const string NotStarted = "Not Started";
                public const string InProgress = "In Progress";
                public const string Completed = "Completed";
            }

        }
    }
}
