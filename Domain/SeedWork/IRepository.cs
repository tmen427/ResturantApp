using Resturant.Domain.Entity;

namespace Resturant.Domain.SeedWork
{

    public interface IRepository
    {
        Task<List<ShoppingCartItems>> ReturnListItemsAsync();
        Task<ShoppingCartItems?> ReturnCartItemsByGuidAsync(string guid);

        Task<int> SaveCartItemsAsync();

        Task<OrderItem?> FindByPrimaryKey(int id);

        Task<List<OrderItem>> ReturnMenuItemsListAsync();
        
        Task<List<OrderItem>> ReturnMenuItemListByGuid(string guidId);

        Task AddShoppingCartItem(ShoppingCartItems shoppingCartItem);

        decimal SubTotalMenuPrice(Guid menuGuid);


    }
}