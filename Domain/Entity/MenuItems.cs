using System.ComponentModel.DataAnnotations;

namespace Resturant.Domain.Entity;



public class MenuItems
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    
    [Required]
    public ShoppingCartItems ShoppingCartItems { get; set; } 
    public  Guid ShoppingCartItemsIdentity{ get; set; }
    
}