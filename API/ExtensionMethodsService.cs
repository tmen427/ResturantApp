using API.Repository;


namespace API;

public static class ExtensionMethodsService
{
    public static void AddMoreServices(this IServiceCollection services)
    {
        services.AddTransient<IRepository, OrderRepo>(); 
    }
}