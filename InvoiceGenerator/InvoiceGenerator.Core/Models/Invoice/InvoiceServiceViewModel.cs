namespace InvoiceGenerator.InvoiceGenerator.Core.Models.Invoice
{
    public class InvoiceServiceViewModel
    {
        public string Id { get; set; }

        public string InvoiceNumber { get; set; }

        public string UserId { get; set; }

        public string ClientName { get; set; }

        public string ClientEmail { get; set; }

        public string ClientPhone { get; set; }

        public string ClientAddress { get; set; }

        public string CompanyName { get; set; }

        public string CompanyEmail { get; set; }

        public string CompanyPhone { get; set; }

        public string CompanyAddress { get; set; }

        public string IssueDate { get; set; }

        public string DueDate { get; set; }

        public string TaxId { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Tax { get; set; }

        public decimal Total { get; set; }

        public string Status { get; set; }

        public List<InvoiceItemViewModel> InvoiceItems { get; set; } = new();
    }

}
