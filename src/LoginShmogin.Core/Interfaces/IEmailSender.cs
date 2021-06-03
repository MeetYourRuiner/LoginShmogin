using System;
using System.Threading.Tasks;
using LoginShmogin.Core.DTOs;

namespace LoginShmogin.Core.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailRequest emailRequest);
    }
}
