using Codeboss.Types;

namespace ChurchManager.Domain.Features.People
{
    #region Enumerations

    /// <summary>
    /// Represents the age classification preference of a <see cref="AgeClassification"/> in a <see cref="Person"/>.
    /// </summary>
    public class AgeClassification : Enumeration<AgeClassification, string>
    {
        public AgeClassification(string value) => Value = value;

        public static AgeClassification Adult = new("Adult");
        public static AgeClassification Child = new("Child");
        public static AgeClassification Unknown = new("Unknown");
        // Implicit conversion from string
        public static implicit operator AgeClassification(string value) => new(value);
    }

    /// <summary>
    /// Represents the age classification preference of a <see cref="Gender"/> in a <see cref="Person"/>.
    /// </summary>
    public class Gender : Enumeration<Gender, string>
    {
        public Gender(string value) => Value = value;

        public static Gender Male = new("Male");
        public static Gender Female = new("Female");
        public static Gender Unknown = new("Unknown");
        // Implicit conversion from string
        public static implicit operator Gender(string value) => new(value);
    }

    /// <summary>
    /// Represents the age classification preference of a <see cref="ConnectionStatus"/> in a <see cref="Person"/>.
    /// </summary>
    public class ConnectionStatus : Enumeration<ConnectionStatus, string>
    {
        public ConnectionStatus(string value) => Value = value;

        public static ConnectionStatus FirstTimer = new("First Timer");
        public static ConnectionStatus NewConvert = new("New Convert");
        public static ConnectionStatus Member = new("Member");
        // Implicit conversion from string
        public static implicit operator ConnectionStatus(string value) => new(value);
    }

    public class MaritalStatus : Enumeration<MaritalStatus, string>
    {
        public MaritalStatus(string value) => Value = value;

        public static MaritalStatus Single = new("Single");
        public static MaritalStatus Married = new("Married");
        public static MaritalStatus Widowed = new("Widowed");
        public static MaritalStatus Divorced = new("Divorced");
        public static MaritalStatus Unknown = new("Unknown");
        // Implicit conversion from string
        public static implicit operator MaritalStatus(string value) => new(value);
    }

    #endregion
}
