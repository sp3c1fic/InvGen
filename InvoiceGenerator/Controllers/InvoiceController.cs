using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Newtonsoft.Json;
using InvoiceGenerator.InvoiceGenerator.Infrastructure.Models;
using InvoiceGenerator.InvoiceGenerator.Core.Contracts;
using Microsoft.AspNetCore.Identity;
using InvoiceGenerator.InvoiceGenerator.Infrastructure;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Globalization;
using InvoiceGenerator.InvoiceGenerator.Core.Models.Invoice;

namespace InvoiceGenerator.Controllers
{
    [Authorize]
    public class InvoiceController: Controller
    {
        private readonly IInvoiceService _invoiceService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly InvoiceGeneratorDbContext _invoiceGeneratorDbContext;

        public InvoiceController(IInvoiceService invoiceService, 
                                    UserManager<User> userManager, 
                                    InvoiceGeneratorDbContext invoiceGeneratorDbContext,
                                    IMapper mapper,
                                    IEmailSenderService emailSenderService)
        {
            _invoiceService = invoiceService;
            _userManager = userManager;
            _invoiceGeneratorDbContext = invoiceGeneratorDbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var invoiceEntity = await _invoiceService.GetInvoiceWithItemsAsync(id);

            if (invoiceEntity == null)
            {
                return BadRequest("Invoice not found");
            }

            var invoiceModel = _mapper.Map<InvoiceServiceViewModel>(invoiceEntity);
            return View(invoiceModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, [FromBody]JsonElement rawJson)
        {
            var model = JsonConvert.DeserializeObject<InvoiceUpdateModel>(rawJson.GetRawText());

            if (model == null)
            {
                return BadRequest("Something went wrong");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            foreach(var item in model.InvoiceItemsList)
            {
                var totalWithTax = item.InvoiceItemUnitPrice * 1.2m;
                item.InvoiceItemTotal = totalWithTax;
            }

            var invoiceToUpdate = await _invoiceService.GetInvoiceWithItemsAsync(id);

            if (invoiceToUpdate == null)
            {
                return BadRequest("Requested invoice does not exist");
            }

            foreach (var invoiceItem in model.InvoiceItemsList)
            {
                var currentItem = invoiceToUpdate.InvoiceItems.FirstOrDefault(i => i.Description == invoiceItem.InvoiceItemDescription);

                if (currentItem == null)
                {
                    invoiceItem.Id = Guid.NewGuid().ToString();
                }
                else
                {
                    invoiceItem.Id = currentItem.Id;
                }
            }

            _invoiceGeneratorDbContext.InvoiceItems.RemoveRange(invoiceToUpdate.InvoiceItems);

            var parsedIssueDate = DateTime.ParseExact(model.IssueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var parsedDueDate = DateTime.ParseExact(model.DueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            invoiceToUpdate.CompanyName = model.CompanyName;
            invoiceToUpdate.ClientAddress = model.CompanyAddress;
            invoiceToUpdate.IssueDate = parsedIssueDate;
            invoiceToUpdate.DueDate = parsedDueDate;
            invoiceToUpdate.ClientName = model.ClientName;

            invoiceToUpdate.InvoiceItems = model.InvoiceItemsList.Select(x => new InvoiceItem
            {
                Id = x.Id,
                InvoiceId = invoiceToUpdate.Id,
                Description = x.InvoiceItemDescription,
                Quantity = x.InvoiceItemQuantity,
                Price = x.InvoiceItemUnitPrice,

            }).ToList();

            invoiceToUpdate.Total = model.InvoiceItemsList.Sum(x => x.InvoiceItemTotal * x.InvoiceItemQuantity);

            try
            {
                var existingIds = _invoiceGeneratorDbContext.InvoiceItems
                                    .Select(x => x.Id)
                                    .ToHashSet();

                var newItemsToAdd = invoiceToUpdate.InvoiceItems
                                    .Where(i => !existingIds.Contains(i.Id))
                                    .ToList();

                await _invoiceGeneratorDbContext.InvoiceItems.AddRangeAsync(newItemsToAdd);
                await _invoiceGeneratorDbContext.SaveChangesAsync();
                Console.WriteLine("Invoice has been successfully updated");
            }
            catch (Exception ex) 
            {
                return BadRequest("Failed to update invoice");
            }

            return Ok(new { success = true, invoiceId = id });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Preview(string id)
        {
            var user = await _userManager.GetUserAsync(User);

            var invoice = await _invoiceGeneratorDbContext.Invoices
                .Include(i => i.InvoiceItems)
                .FirstOrDefaultAsync(i => i.Id == id);

            var invoiceModel = _mapper.Map<InvoiceServiceViewModel>(invoice);

            return View(invoiceModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]JsonElement rawJson)
        {   
            var model = JsonConvert.DeserializeObject<CreateInvoiceViewModel>(rawJson.GetRawText());

            if (model == null)
            {
                return BadRequest("Invalid invoice data");
            }

            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return Unauthorized();
            }

            if (model.DueDate < model.IssueDate)
            {
                return BadRequest($"Due date: {model.DueDate} cannot be earlier than Issue date: {model.IssueDate}");
            }

            // may be someday AUTOMAPPER 

            // AddInvoice

            //var newInvoice = _mapper.Map<Invoice>(model);

            var newInvoice = new Invoice
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                IssueDate = model.IssueDate.Date.ToLocalTime(),
                DueDate = model.DueDate.Date.ToLocalTime(),
                CompanyName = model.CompanyName,
                CompanyPhone = model.CompanyPhone,
                InvoiceNumber = model.InvoiceNumber,
                CompanyAddress = model.CompanyAddress,
                CompanyEmail = model.CompanyEmail!,
                ClientName = model.ClientName!,
                ClientEmail = model.ClientEmail!,
                ClientAddress = model.ClientAddress!,
                ClientPhone = model.ClientPhone!,
                Total = model.Total,
                TaxId = _invoiceService.GenTaxId(), // optional
                Status = "unpaid",
                InvoiceItems = model.invoiceListItems.Select(item => new InvoiceItem
                {
                    Id = Guid.NewGuid().ToString(),
                    Description = item.InvoiceItemDescription,
                    Quantity = item.InvoiceItemQuantity,
                    Price = item.InvoiceItemUnitPrice
                }).ToList()
            };

            // This must be taken out into a function inside of the service class
            await _invoiceGeneratorDbContext.Invoices.AddAsync(newInvoice);
            await _invoiceGeneratorDbContext.SaveChangesAsync();

            return Ok(new { success = true, invoiceId = newInvoice.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Invalid ID.");
            }

            var invoiceToDelete = await _invoiceGeneratorDbContext.Invoices
                .Include(i => i.InvoiceItems)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoiceToDelete == null)
            {
                return BadRequest($"Invoice with id {id} does not exist!.");
            }

            _invoiceGeneratorDbContext.InvoiceItems.RemoveRange(invoiceToDelete.InvoiceItems);
            _invoiceGeneratorDbContext.Invoices.Remove(invoiceToDelete);
            await _invoiceGeneratorDbContext.SaveChangesAsync();

            return Redirect("/Invoice/All");
        }
        
        public async Task<IActionResult> All()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = _userManager.GetUserId(User);
            var allInvoices = await _invoiceService.GetAllInvoices();
            var allUserInvoices = allInvoices.Where(i => i.UserId == userId).ToList();
            return View(allUserInvoices);
        }

    }
}
    