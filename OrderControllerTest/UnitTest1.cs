using API.Controllers;
using API.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Resturant.Domain.Entity;
using Resturant.Infrastructure.Context;

namespace OrderControllerTest;

public class UnitTest1
{
    
    [Fact]
    public async void TestOrderControllerTempCartItemsMethod()
    {
        
        List<TemporaryCartItems> temporaryCartItems = new List<TemporaryCartItems>()
        {
            new TemporaryCartItems() {Id = 1, Indentity = Guid.NewGuid(), Created = DateTime.Now, TotalPrice = 100 },   
            new TemporaryCartItems() {Id = 1, Indentity = Guid.NewGuid(), Created = DateTime.Now, TotalPrice = 100 }, 
            new TemporaryCartItems() {Id = 1, Indentity = Guid.NewGuid(), Created = DateTime.Now, TotalPrice = 100 }, 
        };
  
      var mock = new Mock<IRepository<TemporaryCartItems>>();
      var loggerMock = new Mock<ILogger<OrderController>>();

    
      mock.Setup(m => m.ReturnListItemsAsync())
          .ReturnsAsync(temporaryCartItems);

      var controller = new OrderController(loggerMock.Object, mock.Object);
      var results = await controller.TempCartItems();
   //   var okResult = results.result as OkObjectResult;

      Assert.IsType<List<TemporaryCartItems>>(results);
     // Assert.Equal(200, okResult.StatusCode);
     
      
    }
  
    [Fact]
    public void TestSecondMethod()
    {
        
        var mock = new Mock<IRepository<TemporaryCartItems>>();
        var loggerMock = new Mock<ILogger<OrderController>>();
           mock.Setup(x => x.ReturnCartItemsByGuidAsync(It.IsAny<string>()))
            .ReturnsAsync(new TemporaryCartItems());
        
        var controller = new OrderController(loggerMock.Object, mock.Object);
        var guid = Guid.NewGuid().ToString();
        var results = controller.GetTempsItemsTableByGuid(guid);
        var ok = results.Result as OkObjectResult;
        

        Assert.Equal(200, ok.StatusCode);


    }
    
}