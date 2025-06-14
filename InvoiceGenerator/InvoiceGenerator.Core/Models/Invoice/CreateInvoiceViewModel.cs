using InvoiceGenerator.InvoiceGenerator.Core.Services;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InvoiceGenerator.InvoiceGenerator.Core.Models.Invoice
{
    public class CreateInvoiceViewModel
    {
        [Required]
        [DisplayName("Invoice Number")]
        [JsonPropertyName("invoiceNumber")]
        public string? InvoiceNumber { get; set; }

        [Required]
        [DisplayName("Company Name")]
        [JsonPropertyName("companyName")]
        public string CompanyName { get; set; }

        [Required]
        [DisplayName("Company Phone Number")]
        [RegularExpression(@"^\+?(\d{1,4})?[-.\s]?\(?\d{1,4}\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}$", ErrorMessage = "Invalid phone number")]
        [JsonPropertyName("companyPhone")]
        public string? CompanyPhone { get; set; }

        [Required]
        [DisplayName("Company Address")]
        [JsonPropertyName("companyAddress")]
        public string CompanyAddress { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email")]
        [JsonPropertyName("companyEmail")]
        public string? CompanyEmail { get; set; }

        [DisplayName("Company Tax ID")]
        [JsonPropertyName("companyTaxId")]
        public string? CompanyTaxId { get; set; }

        [Required]
        [DisplayName("Issue Date")]
        [JsonPropertyName("invoiceDate")]
        public DateTime IssueDate { get; set; }

        [Required]
        [DisplayName("Due Date")]
        [JsonPropertyName("dueDate")]
        public DateTime DueDate { get; set; }

        [Required]
        [DisplayName("Client Phone Number")]
        [RegularExpression(@"^\+?(\d{1,4})?[-.\s]?\(?\d{1,4}\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}$", ErrorMessage = "Invalid phone number")]
        [JsonPropertyName("clientPhone")]
        public string? ClientPhone { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Client name cannot exceed 60 characters")]
        [JsonPropertyName("clientName")]
        public string? ClientName { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email")]
        [JsonPropertyName("clientEmail")]
        public string? ClientEmail { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 5)]
        [JsonPropertyName("clientAddress")]
        public string? ClientAddress { get; set; }

        [DisplayName("Client Tax ID")]
        [JsonPropertyName("clientTaxId")]
        public string? ClientTaxId { get; set; }

        [Required]
        [JsonPropertyName("invoiceListItems")]
        public List<CreateInvoiceItemViewModel> invoiceListItems { get; set; } = new();

        [Required]
        [JsonPropertyName("total")]
        public decimal Total { get; set; }
    }

}
