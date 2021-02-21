using Codeboss.Types;

namespace People.Infrastructure.Persistence.Model
{
    #region Enumerations

    /// <summary>
    /// Represents the communication preference of a <see cref="CommunicationType"/> in a <see cref="Person"/>.
    /// </summary>
    public class CommunicationType : Enumeration<CommunicationType, string>
    {
        public CommunicationType(string value) => Value = value;

        public static CommunicationType WhatsApp = new("WhatsApp");
        public static CommunicationType Signal = new("Signal");
        public static CommunicationType Email = new("Email");
        public static CommunicationType SMS = new("SMS");
    }

    /// <summary>
    /// Represents the age classification preference of a <see cref="AgeClassification"/> in a <see cref="Person"/>.
    /// </summary>
    public class AgeClassification : Enumeration<AgeClassification, string>
    {
        public AgeClassification(string value) => Value = value;

        public static AgeClassification Adult = new("Adult");
        public static AgeClassification Child = new("Child");
        public static AgeClassification Unknown = new("Unknown");
    }

    /// <summary>
    /// Represents the age classification preference of a <see cref="Gender"/> in a <see cref="Person"/>.
    /// </summary>
    public class Gender : Enumeration<Gender, string>
    {
        private Gender() { }

        public static Gender Male = new() { Value = "Male" };
        public static Gender Female = new() { Value = "Female" };
        public static Gender Unknown = new() { Value = "Unknown" };
    }

    /// <summary>
    /// Represents the age classification preference of a <see cref="ConnectionStatus"/> in a <see cref="Person"/>.
    /// </summary>
    public class ConnectionStatus : Enumeration<ConnectionStatus, string>
    {
        private ConnectionStatus() { }

        public static ConnectionStatus FirstTimer = new() { Value = "First Timer" };
        public static ConnectionStatus NewConvert = new() { Value = "New Convert" };
        public static ConnectionStatus Member = new() { Value = "Member" };
    }

    public class RecordStatus : Enumeration<RecordStatus, string>
    {
        public static RecordStatus Active = new() { Value = "Active" };
        public static RecordStatus InActive = new() { Value = "InActive" };
        public static RecordStatus Pending = new() { Value = "Pending" };
    }

    #endregion
}
