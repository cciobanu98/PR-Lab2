using Lab2_Web.Interfaces;
using Limilabs.Mail;
using MailKit.Net.Pop3;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace Lab2_Web
{
    public class EmailClient : IEmailClient
    {
        public string Username{get; set;}
        public string Password { get; set; }

        private SmtpClient SmtpClient { get; set; }

        private Pop3Client Pop3Client { get; set; }

        public void Initialize(string username, string password)
        {
            Username = username;
            Password = password;
            SmtpClient = new SmtpClient();
            NetworkCredential nc = new NetworkCredential(Username, Password);
            SmtpClient.Host = "smtp.gmail.com";
            SmtpClient.UseDefaultCredentials = false;
            SmtpClient.Port = 587;
            SmtpClient.Credentials = nc;
            SmtpClient.EnableSsl = true;

            Pop3Client = new Pop3Client();
            Pop3Client.Connect("pop.gmail.com", 995);
            Pop3Client.Authenticate(Username, Password);
        }
        public void SendEmail(string to, string subject, string body)
        {
            MailAddress sendTo = new MailAddress(to);
            MailAddress from = new MailAddress(Username);
            MailMessage message = new MailMessage(from, sendTo);
            message.IsBodyHtml = false;
            message.Subject = subject;
            message.Body = body;
            SmtpClient.Send(message);
        }
        public List<MimeMessage> GetInbox()
        {
            var indexes = Pop3Client.GetMessageCount();
            List<MimeMessage> messages = new List<MimeMessage>();
            for(int i=0;i<3;i++)
            {
                messages.Add(Pop3Client.GetMessage(i));
            }
            return messages;
        }

        public void Close()
        {
            Pop3Client.Disconnect(true);
            SmtpClient.Dispose();
            Username = null;
            Password = null;
        }

        public MimeMessage GetEmail(int id)
        {
            return Pop3Client.GetMessage(id);
        }
    }
}
