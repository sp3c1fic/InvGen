using System.ComponentModel.DataAnnotations;

namespace InvoiceGenerator.InvoiceGenerator.Infrastructure.Models
{
    //Validation please
    public class Invoice
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string UserId { get; set; }

        public User? User { get; set; }

        [Required]
        public string InvoiceNumber { get; set; }

        [Required]
        public DateTime IssueDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string CompanyName { get; set; }

        [Required]
        [RegularExpression(@"^\+?(\d{1,4})?[-.\s]?\(?\d{1,4}\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}$", ErrorMessage = "Invalid phone number")]
        public string CompanyPhone { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string CompanyAddress { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string CompanyEmail { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string ClientName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string ClientEmail { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string ClientAddress { get; set; }

        [Required]
        [RegularExpression(@"^\+?(\d{1,4})?[-.\s]?\(?\d{1,4}\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}$", ErrorMessage = "Invalid phone number")]
        public string ClientPhone { get; set; }

        [Required]
        [Range(0.01, 9999999.99, ErrorMessage = "Total must be greater than zero")]
        public decimal Total { get; set; }

        public string? TaxId { get; set; }

        public string Status { get; set; }

        [Required]
        public ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
    }
}
