using System.Threading.Tasks;
using LoginShmogin.Application.Models;

namespace LoginShmogin.Application.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailRequest emailRequest);
    }
}
