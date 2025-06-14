using MimeKit;

namespace InvoiceGenerator.InvoiceGenerator.Core.Contracts
{
    public interface IEmailSenderService
    {
        Task<MimeMessage> SendEmail(string id, string email);
    }
}
