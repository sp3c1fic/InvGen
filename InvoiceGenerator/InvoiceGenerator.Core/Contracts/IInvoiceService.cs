using InvoiceGenerator.InvoiceGenerator.Core.Models.Invoice;
using InvoiceGenerator.InvoiceGenerator.Infrastructure.Models;

namespace InvoiceGenerator.InvoiceGenerator.Core.Contracts
{
    public interface IInvoiceService
    {
        string GenTaxId();

        Task<IEnumerable<InvoiceServiceViewModel>> GetAllInvoices();

        Task<Invoice> GetInvoiceWithItemsAsync(string id);
    }
}
