namespace Resturant.Domain.Entity;

public class TemporaryCartItems
{
    public int Id{ get; set; }
    
    public Guid Indentity { get; set; }
    
    //public Guid Identity { get; set; }
    public ICollection<MenuItemsVO> MenuItems {get;set;} = new List<MenuItemsVO>();
}


