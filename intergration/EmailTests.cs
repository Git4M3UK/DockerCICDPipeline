using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace intergration
{
    public class EmailTests
    {
        public const string GeneratorApiRoot = "http://generator";
        public const string MailHogApiV2Root = "http://mail:8025/api/v2";

        [Fact]
        public async Task SendEmailWithNames_IsFromGenerator()
        {
            // send email
            var client = new HttpClient();
            var sendEmail = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{GeneratorApiRoot}/EmailRandomNames")
            };

            Console.WriteLine($"****** STEP 1 - Sending email: {sendEmail.RequestUri} ******");

            using (var response = await client.SendAsync(sendEmail))
            {
                response.EnsureSuccessStatusCode();
            }

            // check if email
            var checkEmails = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{MailHogApiV2Root}/messages")
            };

            Console.WriteLine($"****** STEP 2 - Checking emails: {checkEmails.RequestUri} ******");

            using (var response = await client.SendAsync(checkEmails))
            {
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                var messages = JObject.Parse(content);

                messages.Should().HaveElement("total").Which.Should().Be(1);

                messages.Should().HaveElement("items")
                    .Which.Should().BeOfType<JArray>()
                    .Which.First.Should().HaveElement("Raw")
                    .Which.Should().HaveElement("From")
                    .Which.Should().Be("xgenerator@generate.com");
            }
        }
    }
}