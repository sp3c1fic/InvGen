using InvoiceGenerator.InvoiceGenerator.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using System.Security.Claims;

namespace InvoiceGenerator.Controllers
{
    [Authorize]
    public class ChartController : Controller
    {

        private readonly InvoiceGeneratorDbContext _context;

        public ChartController(InvoiceGeneratorDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()    
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null) 
            {
                return BadRequest("Error fetching current user");
            }

            //if (user.IsPaid == false)
            //{
            //    return Redirect("/Payment/Index");
            //}

            var monthlyRevenue = _context.Invoices
                .Where(i => i.Status == "unpaid")
                .GroupBy(i => i.IssueDate.Month)
                .Select(g => new {
                    month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(g.Key),
                    amount = g.Sum(i => i.Total)
                })
                .ToList();

            ViewBag.MonthlyRevenueData = JsonConvert.SerializeObject(monthlyRevenue);


            return View();
        }
    }
}
