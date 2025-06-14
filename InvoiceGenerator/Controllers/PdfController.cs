using InvoiceGenerator.InvoiceGenerator.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceGenerator.Controllers
{
    public class PdfController : ControllerBase
    {
        private readonly IPdfTemplateService _pdfService;
        private readonly IInvoiceService _invoiceService;

        public PdfController(IPdfTemplateService pdfService, IInvoiceService invoiceService)
        {
            _pdfService = pdfService;
            _invoiceService = invoiceService;
        }

        [HttpGet]
        public async Task<IActionResult> ExportInvoice(string id)
        {
            var invoice = await _invoiceService.GetInvoiceWithItemsAsync(id);
            if (invoice == null) return NotFound();

            var pdfBytes = _pdfService.GenerateInvoicePdf(invoice);

            return File(pdfBytes, "application/pdf", $"Invoice_{invoice.InvoiceNumber}.pdf");
        }

    }

}
