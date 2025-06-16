using API.Repository;
using Resturant.Infrastructure.Context;

namespace API;

public class ImplementBackgroundSerivce :BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ImplementBackgroundSerivce> _logger;
    
    
    public ImplementBackgroundSerivce(ILogger<ImplementBackgroundSerivce> logger, IServiceProvider serviceProvider)
    {  
        _logger = logger;
        _serviceProvider = serviceProvider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //you have to wrap it in while loop or else won't work s
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
            _logger.LogCritical("Background Service is running.");

            using (var serviceScope = _serviceProvider.CreateScope())
            { 
                var processingscope = serviceScope.ServiceProvider.GetRequiredService<IRepository>();
                var items = await processingscope.ReturnListItemsAsync();
                _logger.LogInformation(items.Count.ToString());
            }
        }

    }
}
