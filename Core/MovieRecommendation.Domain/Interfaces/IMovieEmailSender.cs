using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRecommendation.Domain.Interfaces
{
    public interface IMovieEmailSender
    {
        Task SendEmail(string fromEmail, string toEmail, string recommendation);
    }
}
