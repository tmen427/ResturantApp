using Resturant.Domain.Entity;

namespace API.Repository;

public interface IRepository 
{
    Task<List<TemporaryCartItems>> ReturnListItemsAsync(); 
    Task<TemporaryCartItems> ReturnCartItemsByGuidAsync(string guid);
    
    Task<int> SaveCartItemsAsync();

    Task<MenuItemsVO> FindByPrimaryKey(int id); 
    


}