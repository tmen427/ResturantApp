using Microsoft.EntityFrameworkCore;
using Resturant.Application.DTO;
using Resturant.Domain.Entity;
using Resturant.Domain.SeedWork;
using Resturant.Infrastructure.Context;

namespace Resturant.Infrastructure.Repository;

public class ShoppingCartRepo : IRepository
{
      private readonly RestaurantContext _context; 
   
    public ShoppingCartRepo(RestaurantContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

    }
    
    public async Task<List<ShoppingCartItems>> ReturnListItemsAsync()
    {
        var tempCartItems = await  _context.
            ShoppingCartItems.Include("MenuItems").ToListAsync();
        return tempCartItems;
    }
    
    public async Task<ShoppingCartItems?> ReturnCartItemsByGuidAsync(string guid)
    {
        var tempItemPrice = await _context.
            ShoppingCartItems.FirstOrDefaultAsync(x=>x.Identity.ToString() == guid);
        return tempItemPrice ?? null; 
    }
    
    public async Task<MenuItems?> FindByPrimaryKey(int id)
    {     
        var menuItem =  await _context.MenuItems.FindAsync(id); 
        return menuItem;
    }
    
    public async Task<List<MenuItems>> ReturnMenuItemsListAsync()
    {
        //respository pattern should only return primitives---not any dtos
        return await _context.ShoppingCartItems.Include("MenuItems")
            .Where(x => x.Identity.ToString() != string.Empty)
            .SelectMany(x => x.MenuItems).ToListAsync();
        //   .Select(x => new MenuDTO() { Id = x.Id, Name = x.Name, Price = x.Price, GuidId = x.ShoppingCartItemsIdentity.ToString() }).ToListAsync();
    }

    public async Task<List<MenuItems>> ReturnMenuItemListByGuid(string guidId)
    {
        
        var shoppingCart =   await _context.ShoppingCartItems.Include("MenuItems")
            .Where(x => x.Identity.ToString() == guidId)
            .SelectMany(x => x.MenuItems).ToListAsync();
        
        return shoppingCart;
      //      .Select(x => new MenuDTO()
        //        { Id = x.Id, Name = x.Name, Price = x.Price, GuidId = x.ShoppingCartItemsIdentity.ToString() })
        //    .ToListAsync();
  
    }

    public async Task AddShoppingCartItem(ShoppingCartItems shoppingCartItem)
    {
        await _context.AddAsync(shoppingCartItem);
    }
    
    
    public decimal SubTotalMenuPrice(Guid menuGuid)
    {
        
       return _context.ShoppingCartItems.Include("MenuItems").
            Where(x => x.Identity == menuGuid).
            SelectMany(x=>x.MenuItems).
            Sum(x => x.Price);
    }
    
    public async Task<int> SaveCartItemsAsync()
    {
        return await _context.SaveChangesAsync();
    }
    
}