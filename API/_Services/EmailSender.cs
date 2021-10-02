using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using API._Helpers;
using API._Interfaces;
using AutoMapper.Configuration;
using Microsoft.Extensions.Options;

namespace API._Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IOptions<SMTP> config;

        public EmailSender(IOptions<SMTP> config)
        {
            this.config = config;
        }

        public async Task SendEmailAsync(string fromAddress, string toAddress, string subject, string message)
        {
            var mailMessage = new MailMessage(fromAddress, toAddress, subject, message);

            using (var client = new SmtpClient(this.config.Value.Host, int.Parse(this.config.Value.Port))
            {
                Credentials = new NetworkCredential(this.config.Value.Username, this.config.Value.Password)
            })
            {
                await client.SendMailAsync(mailMessage);
            }
        }
    }
}