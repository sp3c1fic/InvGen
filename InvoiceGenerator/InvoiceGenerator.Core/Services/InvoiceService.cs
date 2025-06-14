using InvoiceGenerator.InvoiceGenerator.Core.Contracts;
using InvoiceGenerator.InvoiceGenerator.Core.Models.Invoice;
using InvoiceGenerator.InvoiceGenerator.Infrastructure;
using InvoiceGenerator.InvoiceGenerator.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace InvoiceGenerator.InvoiceGenerator.Core.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly InvoiceGeneratorDbContext _invoiceGeneratorDbContext;
        private readonly UserManager<User> _userManager;

        public InvoiceService(InvoiceGeneratorDbContext invoiceGeneratorDbContext, UserManager<User> userManager)
        {
            _invoiceGeneratorDbContext = invoiceGeneratorDbContext;
            _userManager = userManager;
        }

        public string GenTaxId()
        {
            var random = new Random();
            return $"TAX-{random.Next(100000000, 999999999)}";
        }
        public async Task<IEnumerable<InvoiceServiceViewModel>> GetAllInvoices()
        {

            var allInvoices = await _invoiceGeneratorDbContext.Invoices
                
                .Select(i => new InvoiceServiceViewModel
            {
                Id = i.Id,
                InvoiceNumber = i.InvoiceNumber,
                ClientName = i.ClientName,
                IssueDate = i.IssueDate.ToString("d"),
                DueDate = i.DueDate.ToString("d"),
                Total = i.Total,
                Status = "unpaid",
                UserId = i.UserId
            })
            .ToListAsync();

            return allInvoices;
        }

        public async Task<Invoice> GetInvoiceWithItemsAsync(string id)
        {
            var result = await _invoiceGeneratorDbContext.Invoices
                .Include(i => i.InvoiceItems)
                .FirstOrDefaultAsync(i => i.Id == id);
            return result!;
        }
        

        
    }
}
    