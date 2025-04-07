using RealEstate.Infraestructure.Core;
using RealEstate.Infraestructure.Dtos;
using RealEstate.Infraestructure.Settings;

namespace RealEstate.Infraestructure.Interfaces
{
    public interface IEmailService
    {
        public MailSettings MailSettings { get; }
        Task<NotificationResponse> SendEmailAsync(EmailRequest emailRequest);
    }
}
