namespace InvoiceGenerator.InvoiceGenerator.Core.Models
{
    public class SendEmailServiceModel
    {
        public string Id { get; set; }
        public string Recipient { get; set; }
        public string InvoiceNumber { get; set; }
        public string Notes { get; set; }
    }
}
