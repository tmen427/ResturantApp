using System.ComponentModel.DataAnnotations;

namespace Resturant.Domain.Entity;


//this is not a value object since it has it's own id
public class MenuItemsVO
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    
    [Required]
    public TemporaryCartItems TemporaryCartItems { get; set; } 
    public  Guid TemporaryCartItemsIndentity{ get; set; }
    
}