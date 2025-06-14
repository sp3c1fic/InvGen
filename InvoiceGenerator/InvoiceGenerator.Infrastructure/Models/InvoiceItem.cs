using System.ComponentModel.DataAnnotations;

namespace InvoiceGenerator.InvoiceGenerator.Infrastructure.Models
{
    public class InvoiceItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string InvoiceId { get; set; }

        public Invoice? Invoice { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Description must be between 2 and 200 characters")]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 10000, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        public decimal Total => Quantity * Price;
    }
}
