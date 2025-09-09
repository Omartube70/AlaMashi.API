﻿namespace Application.Common.Interfaces
{
    public class EmailSettings
    {
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
    }
}