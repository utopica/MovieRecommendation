using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MovieRecommendation.Domain.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MovieRecommendation.Persistence.Services
{
    public class MovieEmailSender : IMovieEmailSender
    {
        private readonly string _apiKey;

        public MovieEmailSender(IConfiguration configuration)
        {
            _apiKey = configuration["SendGrid:ApiKey"];
        }

        public async Task SendEmail(string fromEmail, string toEmail, string recommendation)
        {
            var apiKey = _apiKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, "Me");
            var subject = $"Movie Recommendation : {recommendation}";
            var to = new EmailAddress(toEmail, "Dear");
            var plainTextContent = "You have been recommended a movie. Please check it out.";
            var htmlContent = "";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
