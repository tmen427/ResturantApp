using System.ComponentModel.DataAnnotations;

namespace Resturant.Domain.Entity;



public class MenuItem
{
    public int Id { get; set; }
    public string MenuItemName { get; set; } 
    
    public string MenuItemDescription { get; set; }
    
    public string MenuItemImageUrl { get; set; } = string.Empty;
    public decimal MenuItemBasePrice { get;  set; }
    
    
}