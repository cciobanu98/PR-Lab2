using MimeKit;
using System.Collections.Generic;

namespace Lab2_Web.Interfaces
{
    public interface IEmailClient
    {
        string Username { get; set; }

        string Password { get; set; }
        public void Initialize(string username, string password);

        public List<MimeMessage> GetInbox();

        public void SendEmail(string to, string subject, string body);

        public MimeMessage GetEmail(int id);

        public void Close();
    }
}
