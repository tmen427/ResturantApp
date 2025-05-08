namespace API.Repository;

public interface IRepository<T> where T : class
{
    Task<List<T>> ReturnListItemsAsync(); 
    Task<T> ReturnCartItemsByGuidAsync(string guid);
}