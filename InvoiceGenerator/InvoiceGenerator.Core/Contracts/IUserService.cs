using InvoiceGenerator.InvoiceGenerator.Infrastructure.Models;

namespace InvoiceGenerator.InvoiceGenerator.Core.Contracts
{
    public interface IUserService
    {
        Task<User> GetCurrentUser();
    }
}
