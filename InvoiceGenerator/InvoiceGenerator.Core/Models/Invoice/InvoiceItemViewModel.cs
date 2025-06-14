namespace InvoiceGenerator.InvoiceGenerator.Core.Models.Invoice
{
    public class InvoiceItemViewModel
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Total => Quantity * Price;
    }
}
