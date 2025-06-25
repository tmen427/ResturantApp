using System.ComponentModel.DataAnnotations;

namespace Resturant.Domain.Entity
{
    public class CustomerInquiryInformation
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }
    }
}
