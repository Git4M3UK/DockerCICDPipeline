using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Faker;
using MailKit.Security;
using MailKit.Net.Smtp;
using MimeKit;

namespace api.Controllers
{
    // Just use action name as route
    [Route("[action]")]
    public class GenerateController : Controller
    {
        // The MAIL_HOST name is taken from the service name in docker-compose.yml
        // To run this using IIS you would need to add an entry in the host file for mail 127.0.0.1
        public const string MailHost = "mail"; 
        public const int MailPort = 1025;

        [HttpPost]
        public async Task EmailRandomNames(Range range, string email = "test@fake.com")
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Generator", "generator@generate.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Here are some random names for you 3";

            message.Body = new TextPart("plain")
            {
                Text = string.Join(Environment.NewLine, range.Of(Name.FullName))
            };
            using (var mailClient = new SmtpClient())
            {
                await mailClient.ConnectAsync(MailHost, MailPort, SecureSocketOptions.None);
                await mailClient.SendAsync(message);
                await mailClient.DisconnectAsync(true);
            }
        }

        [HttpGet]
        public IEnumerable<string> Names(Range range)
            => range.Of(Name.FullName);

        [HttpGet]
        public IEnumerable<string> PhoneNumbers(Range range)
            => range.Of(Phone.Number);

        [HttpGet]
        public IEnumerable<int> Numbers(Range range)
            => range.Of(RandomNumber.Next);

        [HttpGet]
        public IEnumerable<string> Companies(Range range)
            => range.Of(Company.Name);

        [HttpGet]
        public IEnumerable<string> Paragraphs(Range range)
            => range.Of(() => Lorem.Paragraph(3));

        [HttpGet]
        public IEnumerable<string> CatchPhrases(Range range)
            => range.Of(Company.CatchPhrase);

        [HttpGet]
        public IEnumerable<string> Marketing(Range range)
            => range.Of(Company.BS);

        [HttpGet]
        public IEnumerable<string> Emails(Range range)
            => range.Of(Internet.Email);

        [HttpGet]
        public IEnumerable<string> Domains(Range range)
            => range.Of(Internet.DomainName);
    }
}
