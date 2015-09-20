using System;

namespace MotorDepot.WEB.Models
{
    public class TempMessage
    {
        public TempMessageType Type { get; set; }
        public String Message { get; set; }

        public TempMessage(TempMessageType type, string message)
        {
            Type = type;
            Message = message;
        }
    }

    public class LinkTempMessage : TempMessage
    {
        public String LinkText { get; set; }
        public String LinkHref { get; set; }

        public LinkTempMessage(TempMessageType type, string message, string linkText, string linkHref) : base(type, message)
        {
            LinkText = linkText;
            LinkHref = linkHref;
        }
    }

    public enum TempMessageType
    {
        Info,
        Error,
        Success
    }
}