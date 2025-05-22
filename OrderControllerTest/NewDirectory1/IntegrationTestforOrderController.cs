using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace OrderControllerTest.NewDirectory1;

public class IntegrationTestforOrderController : IClassFixture<WebApplicationFactory<Program>>
{
    
    private readonly HttpClient _client;
    
    public IntegrationTestforOrderController(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    
    
    [Theory]
    [InlineData("api/Order/GetTotalPrice?guid=095e1dd5-22bd-45d0-b2c4-4368bf14d8fb")]
    [InlineData("/api/order/TempItemsTable")]
    public async Task GetEndPointsResturnSuccessandCorrectStatusCodes(string url)
    {
        var response  = await _client.GetAsync(url); 
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
  
    
    
    
}