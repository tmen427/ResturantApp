using System.Text.Json.Serialization;

namespace Resturant.Domain.Entity;

public class ShoppingCartItems
{
    public int Id{ get; set; }
    public Guid Identity { get; set; }
    public DateTime Created { get; set; }
    public decimal TotalPrice {get;set;}

    public List<MenuItems> MenuItems { get; set; } = new List<MenuItems>();

    //realtionship needs to be made with customerinformation
    public int? CustomerInformationId { get; set; }
    public CustomerInformation? CustomerInformation { get; set; } 

}


