using API.Repository;
using Resturant.Application.Respository;

namespace API;

public static class ExtensionMethodsService
{
    public static void AddMoreServices(this IServiceCollection services)
    {
        services.AddTransient<IRepository, OrderRepo>(); 
    }
}