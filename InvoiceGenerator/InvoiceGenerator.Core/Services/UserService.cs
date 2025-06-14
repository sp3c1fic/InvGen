using InvoiceGenerator.InvoiceGenerator.Core.Contracts;
using InvoiceGenerator.InvoiceGenerator.Infrastructure;
using InvoiceGenerator.InvoiceGenerator.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InvoiceGenerator.InvoiceGenerator.Core.Services
{

    public class UserService : IUserService
    {
        private readonly InvoiceGeneratorDbContext _context;

        public UserService(InvoiceGeneratorDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetCurrentUser()
        {
            throw new NotImplementedException();
        }
    }
}
