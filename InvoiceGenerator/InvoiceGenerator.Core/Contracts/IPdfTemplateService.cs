using InvoiceGenerator.InvoiceGenerator.Infrastructure.Models;

namespace InvoiceGenerator.InvoiceGenerator.Core.Contracts
{
    public interface IPdfTemplateService
    {
        byte[] GenerateInvoicePdf(Invoice invoice);

    }
}
