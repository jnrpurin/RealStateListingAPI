using System.ComponentModel.DataAnnotations;

namespace RealEstateListingApi.Models
{
    public class Listing
    {
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();  // Default to empty string if nulls aren't allowed

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        [Range(1, double.MaxValue)]
        public decimal Price { get; set; }  // Decimal is a value type and non-nullable by default

        [StringLength(500)]
        public string? Description { get; set; }  // Mark as nullable if appropriate
    }
}

