using ApplicationGateway.Application.Models.Mail;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Contracts.Infrastructure
{
    public interface IEmailService
    {
        Task<bool> SendEmail(Email email);
    }
}
