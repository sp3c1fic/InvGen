using System.Text.Json.Serialization;

namespace InvoiceGenerator.InvoiceGenerator.Core.Models.Invoice
{
    public class InvoiceUpdateModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("companyName")]
        public string CompanyName { get; set; }

        [JsonPropertyName("companyAddress")]
        public string CompanyAddress { get; set; }

        [JsonPropertyName("issueDate")]
        public string IssueDate { get; set; }

        [JsonPropertyName("dueDate")]
        public string DueDate { get; set; }

        [JsonPropertyName("clientName")]
        public string ClientName { get; set; }

        [JsonPropertyName("invoiceItemsList")]
        public List<InvoiceItemUpdateModel> InvoiceItemsList { get; set; }

        [JsonPropertyName("total")]
        public string Total { get; set; }
    }
}
