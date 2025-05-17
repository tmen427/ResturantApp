using API.DTO;
using Microsoft.EntityFrameworkCore;
using Resturant.Domain.Entity;
using Resturant.Infrastructure.Context;

namespace API.Repository;

public class OrderRepo : IRepository<TemporaryCartItems>
{
    
    //the Order repo can also include other methods that are not included in IRepository 
    
    private  readonly ToDoContext _context; 
    
    //asp.net automatically inititializes this object 
    public OrderRepo(ToDoContext context)
    {
        _context = context;
    }
    
    public async Task<List<TemporaryCartItems>> ReturnListItemsAsync()
    { 
        var tempCartItems = await _context.
            TemporaryCartItems.Include("MenuItems").ToListAsync();
        return tempCartItems;
    }

    public async Task<TemporaryCartItems> ReturnCartItemsByGuidAsync(string guid)
    {
        var tempItemPrice = await _context.
            TemporaryCartItems.FirstOrDefaultAsync(x=>x.Indentity.ToString() == guid);
        return tempItemPrice;
    }

    public Task<List<MenuDTO>> ReturnCartItemsAsync()
    {
        throw new NotImplementedException();
    }
    
}