
namespace Resturant.Application.Respository
{
    public interface IRepo<T> where T : class 
    {


        Task<List<T>> CartItemsAsync(); 

        Task<T> PostItemsAsync(T object_name);
    }
}
