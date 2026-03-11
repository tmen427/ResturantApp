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
        ArgumentException.ThrowIfNullOrEmpty(nameof(context));
        _context = context; 

    }
    
    public async Task<List<ShoppingCartItems>> ReturnListItemsAsync()
    
    {
        

        var tempCartItems = await  _context.
            ShoppingCartItems.Include("MenuItems").ToListAsync();
        return tempCartItems;
    }
    
    public async Task<ShoppingCartItems?> ReturnCartItemsByGuidAsync(string guid)
    {
        ShoppingCartItems? tempItemPrice = await _context.
            ShoppingCartItems.FirstOrDefaultAsync(x=>x.Identity.ToString() == guid);
        return tempItemPrice ?? null; 
    }
    
    public async Task<OrderItem?> FindByPrimaryKey(int id)
    {     
        var menuItem =  await _context.OrderItem.FindAsync(id); 
        return menuItem;
    }
    
    public async Task<List<OrderItem>> ReturnMenuItemsListAsync()
    {
        //respository pattern should only return primitives---not any dtos
        return await _context.ShoppingCartItems
            .Where(x => x.Identity.ToString() != string.Empty)
            .SelectMany(x => x.OrderItems).ToListAsync();
            //.Select(x => new MenuDTO() { Id = x.Id, Name = x.Name, Price = x.Price, GuidId = x.ShoppingCartItemsIdentity.ToString() }).ToListAsync();
            
    }

    public async Task<List<OrderItem>>  ReturnMenuItemListByGuid(string guidId)
    {

        var shoppingCart = await _context.ShoppingCartItems
            .Where(x => x.Identity.ToString() == guidId)
            .SelectMany(x => x.OrderItems
            ).Include(x=>x.Options).ToListAsync();

       return shoppingCart;
   
        
    }

    public async Task AddShoppingCartItem(ShoppingCartItems shoppingCartItem)
    {
        await _context.AddAsync(shoppingCartItem);
    }
    
    
    //calculates all menu time prices
    public decimal SubTotalMenuPrice(Guid menuGuid)
    {
        return _context.ShoppingCartItems.Where(x => x.Identity == menuGuid).
            SelectMany(x=>x.OrderItems).
            Sum(x => x.TotalItemPrice);
        
    }
    
    public async Task<int> SaveCartItemsAsync()
    {
        return await _context.SaveChangesAsync();
    }
    
}