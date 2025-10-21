namespace Application.Common.Interfaces
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public bool EnableSsl { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}