using Resturant.Domain.Entity;

namespace Resturant.Application.DTO
{
    public class CustomerDto
    {
        public string? Credit { get; set; }
        public string? NameonCard { get; set; }
        public string? CreditCardNumber { get; set; }
        public string? Expiration { get; set; }
        public string? CVV { get; set; }
        public string? UserName { get; set; }     
        
        public string? GuidId { get; set;  }
      //  public List<MenuItemsVO> MenuItems {get;set;} = new List<MenuItemsVO>();
    }
}
