
namespace Resturant.Application.Respository
{
    public interface IRepo1<T> where T : class 
    {


        Task<List<T>> CartItemsAsync(); 

        Task<T> PostItemsAsync(T t);

        Task<List<T>> SearchByName(string name); 


    }
}
