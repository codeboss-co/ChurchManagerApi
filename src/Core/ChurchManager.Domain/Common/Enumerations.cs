﻿using Codeboss.Types;

namespace ChurchManager.Domain.Common
{
    public class RecordStatus : Enumeration<RecordStatus, string>
    {
        public RecordStatus(string value) => Value = value;

        public static RecordStatus Active = new("Active");
        public static RecordStatus InActive = new("InActive");
        public static RecordStatus Pending = new("Pending");
        // Implicit conversion from string
        public static implicit operator RecordStatus(string value) => new(value);
        // Implicit conversion to bool
        public static implicit operator bool(RecordStatus status) =>
            status != null && !string.IsNullOrEmpty(status.Value) && status.Value == Active;
    }

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
        public static CommunicationType None = new("None");

        public static implicit operator CommunicationType(string value) => new(value);
    }
}
