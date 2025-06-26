using Resturant.Domain.Entity;

namespace Resturant.Domain.SeedWork
{

    public interface IRepository
    {
        Task<List<ShoppingCartItems>> ReturnListItemsAsync();
        Task<ShoppingCartItems?> ReturnCartItemsByGuidAsync(string guid);

        Task<int> SaveCartItemsAsync();

        Task<MenuItems?> FindByPrimaryKey(int id);

        Task<List<MenuItems>> ReturnMenuItemsListAsync();
        
        Task<List<MenuItems>> ReturnMenuItemListByGuid(string guidId);

        Task AddShoppingCartItem(ShoppingCartItems shoppingCartItem);

        decimal TotalMenuPrice(Guid menuGuid);


    }
}