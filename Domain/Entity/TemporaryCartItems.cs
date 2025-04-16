using System.Text.Json.Serialization;

namespace Resturant.Domain.Entity;

public class TemporaryCartItems
{
    public int Id{ get; set; }
    
    public Guid Indentity { get; set; }
    
    public DateTime Created { get; set; }
    
    public List<MenuItemsVO> MenuItems {get;set;} = new List<MenuItemsVO>();
    
    //child to parent - one to one 
  //  public OrderInformation OrderInformation { get; set; } = null!;
  //  public int? OrderInformationId { get; set; } 
}


