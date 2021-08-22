using System.IO;

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

        public static class Communication
        {
            public static class Email
            {
                public static string TemplatePath = Path.Combine("Email", "Templates");
                public static string TemplateExtension = ".liquid";

                public static string FollowUpTemplate = "FollowUpAssignment";

                public static string Template(string name) => Path.Combine(TemplatePath, $"{name}{TemplateExtension}");
            }
        }
    }
}
