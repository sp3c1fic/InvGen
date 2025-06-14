using Microsoft.AspNetCore.Identity;

namespace InvoiceGenerator.InvoiceGenerator.Infrastructure.Models
{
    public class User: IdentityUser
    {
        public bool IsPaid { get; set; } = false;

        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    }
}
