using System;

namespace NVHplatform.Models
{
    public class LogMessage
    {
        public DateTime Timestamp { get; set; }
        public string Content { get; set; }
        public LogLevel Level { get; set; }
    }

    public enum LogLevel
    {
        Info,
        Warning,
        Error
    }
}
