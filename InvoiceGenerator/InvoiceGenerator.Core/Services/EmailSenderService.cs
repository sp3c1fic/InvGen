using AutoMapper;
using MimeKit;
using Scriban;

using InvoiceGenerator.InvoiceGenerator.Core.Contracts;
using InvoiceGenerator.InvoiceGenerator.Core.Models.Invoice;
using InvoiceGenerator.InvoiceGenerator.Infrastructure.Models;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace InvoiceGenerator.InvoiceGenerator.Core.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IMapper _mapper;

        public EmailSenderService(IInvoiceService invoiceService, IMapper mapper) 
        {
            _invoiceService = invoiceService;
            _mapper = mapper;
        }
        public async Task<MimeMessage> SendEmail(string id, string email)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Mr Duracell", "xr4nomad@gmx.co.uk"));
            message.To.Add(new MailboxAddress($"{email}", email));
            message.Subject = "Invoice";

            var renderedHtml = await RenderInvoiceHtml(id, "Templates/invoice_email.sbnhtml");

            var builder = new BodyBuilder
            {
                HtmlBody = renderedHtml
            };

            message.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            smtp.AuthenticationMechanisms.Remove("XOAUTH2");
            await smtp.ConnectAsync("smtp.abv.bg", 465, SecureSocketOptions.SslOnConnect);
            await smtp.AuthenticateAsync("", "");
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);

            return message;
        }

        private async Task<string> RenderInvoiceHtml(string id, string templatePath)    
        {
            var invoiceToSend = await _invoiceService.GetInvoiceWithItemsAsync(id);
            var invoiceModel = _mapper.Map<InvoiceServiceViewModel>(invoiceToSend);

            var templateText = File.ReadAllText(templatePath);
            var template = Template.Parse(templateText);

            var items = invoiceModel.InvoiceItems.Select(i => new { description = i.Description, amount = i.Quantity });
            var html = template.Render(new
            {
                client_name = invoiceModel.ClientName,
                client_email = invoiceModel.ClientEmail,
                invoice_number = invoiceModel.InvoiceNumber,
                invoice_date = invoiceModel.IssueDate,
                items,
                total = invoiceModel.Total
            });

            return html;
        }

        private static async Task<string> GenerateEmailHtml(Invoice invoice)
        {
            return null;
        }

    }
}
