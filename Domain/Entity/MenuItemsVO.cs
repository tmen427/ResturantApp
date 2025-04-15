using System.ComponentModel.DataAnnotations;

namespace Resturant.Domain.Entity;


//this is not a value object since it has it's own id
public class MenuItemsVO
{
    public int Id { get; set; }
   // public int Id { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
    

    
  // public int TemporaryCartItemsId { get; set; } 
  [Required]
  public TemporaryCartItems TemporaryCartItems { get; set; } 
   public  Guid TemporaryCartItemsIndentity{ get; set; }
   

    // public MenuItemsVO(string name, double price)
    // {
    //     Name = name;
    //     Price = price;
    // }
}