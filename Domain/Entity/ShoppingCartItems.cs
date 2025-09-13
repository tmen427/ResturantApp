using System.Text.Json.Serialization;

namespace Resturant.Domain.Entity;

public class ShoppingCartItems
{
    public int Id{ get; set; }
    public Guid Identity { get; set; }
    public DateTime Created { get; set; }
    
    public decimal SubTotal { get; set; }

    public decimal TaxRate { get; set; } = 0.06875m; 
    
    public decimal TaxAmount { get; set; }
    
    public decimal TotalPrice {get;set;}

    public List<MenuItems> MenuItems { get; set; } = new List<MenuItems>();

    public int? CustomerInformationId { get; set; }

    public CustomerPaymentInformation? CustomerInformation { get; set; } 

}


