using API.DTO;
using Microsoft.EntityFrameworkCore;
using Resturant.Domain.Entity;
using Resturant.Infrastructure.Context;

namespace API.Repository;

public class OrderRepo : IRepository
{
    
    private readonly ToDoContext _context; 
    private readonly ILogger<OrderRepo> _logger;
    public OrderRepo(ToDoContext context, ILogger<OrderRepo> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task<List<TemporaryCartItems>> ReturnListItemsAsync()
    { 
        var tempCartItems = await _context.
            TemporaryCartItems.Include("MenuItems").ToListAsync();
        return tempCartItems;
        
    }

    public async Task<TemporaryCartItems?> ReturnCartItemsByGuidAsync(string guid)
    {
        var tempItemPrice = await _context.
            TemporaryCartItems.FirstOrDefaultAsync(x=>x.Indentity.ToString() == guid);
        
        return tempItemPrice ?? null; 
    }

    public async Task<int> SaveCartItemsAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<MenuItemsVO> FindByPrimaryKey(int id)
    {     
        var menuItem =  await _context.MenuItems.FindAsync(id); 
        return menuItem!;
    }
}