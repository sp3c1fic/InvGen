using System.Text.Json.Serialization;

namespace InvoiceGenerator.InvoiceGenerator.Core.Models.Invoice
{
    public class InvoiceItemUpdateModel
    {
        public string Id { get; set; }

        [JsonPropertyName("invoiceItemDescription")]
        public string InvoiceItemDescription { get; set; }

        [JsonPropertyName("invoiceItemQuantity")]
        public int InvoiceItemQuantity { get; set; }

        [JsonPropertyName("invoiceItemUnitPrice")]
        public decimal InvoiceItemUnitPrice { get; set; }

        [JsonPropertyName("invoiceItemTax")]
        public decimal InvoiceItemTax { get; set; }

        [JsonPropertyName("invoiceItemTotal")]
        public decimal InvoiceItemTotal { get; set; }
    }
}
