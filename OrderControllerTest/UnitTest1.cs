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
      var callMethod = await controller.TempCartItems();

      Assert.NotNull(mock);
      Assert.NotNull(callMethod);
      
    }
  
    [Fact]
    public void TestSecondMethod()
    {
        var g = Guid.NewGuid().ToString();
        TemporaryCartItems t = new TemporaryCartItems(); 
        var mock = new Mock<IRepository<TemporaryCartItems>>();
        var items = mock.Setup(m => m.ReturnCartItemsByGuidAsync(It.IsAny<Guid>())).ReturnsAsync(TemporaryCartItems);


    }
    
}