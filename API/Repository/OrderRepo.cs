using API.DTO;
using Microsoft.EntityFrameworkCore;
using Resturant.Domain.Entity;
using Resturant.Infrastructure.Context;

namespace API.Repository;

public class OrderRepo : IRepository
{

    private readonly ToDoContext _context; 

    public OrderRepo(ToDoContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    
    }
    
    public async Task<List<TemporaryCartItems>> ReturnListItemsAsync()
    { 
        var tempCartItems = await  _context.
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
        MenuItemsVO? menuItem =  await _context.MenuItems.FindAsync(id); 
        return menuItem;
    }
    
    public async Task<List<MenuDTO>> ReturnMenuDtoListAsync()
    {
        var menuDto = await _context.TemporaryCartItems.Include("MenuItems")
            .Where(x => x.Indentity.ToString() != string.Empty)
            .SelectMany(x => x.MenuItems)
            .Select(x => new MenuDTO() { Id = x.Id, Name = x.Name, Price = x.Price, GuidId = x.TemporaryCartItemsIndentity.ToString() }).ToListAsync();
        
        return menuDto;
    }
    
}