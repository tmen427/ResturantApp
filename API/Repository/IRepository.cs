using API.DTO;
using Resturant.Domain.Entity;

namespace API.Repository;

public interface IRepository 
{
    Task<List<TemporaryCartItems>> ReturnListItemsAsync(); 
    Task<TemporaryCartItems?> ReturnCartItemsByGuidAsync(string guid);
    
    Task<int> SaveCartItemsAsync();

    Task<MenuItemsVO> FindByPrimaryKey(int id); 
    
    Task<List<MenuDTO>> ReturnMenuDtoListAsync();
    
    //therew no rule that says you have to return one type in a ipresotiry pattern
    Task<List<MenuDTO>> ReturnListMenuDtoListByGuid(string guidId);

    decimal TotalMenuPrice(Guid menuGuid); 


}