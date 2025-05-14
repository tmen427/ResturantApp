using API.Controllers;
using API.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
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
      var dbocontext = new Mock<ToDoContext>(); 
    
      mock.Setup(m => m.ReturnListItemsAsync())
          .ReturnsAsync(temporaryCartItems);

      var controller = new OrderController(loggerMock.Object, dbocontext.Object,  mock.Object);
      var results = await controller.TempCartItems();
      var okResult = results as OkObjectResult;


      Assert.IsAssignableFrom<IActionResult>(okResult);
      Assert.Equal(200, okResult.StatusCode);
      
    }

   [Fact]
    public void UnitTestDivisionByZero()
    {
        var mock = new Mock<IRepository<TemporaryCartItems>>(); 
        var loggerMock = new Mock<ILogger<OrderController>>();
        var dbcontext = new Mock<ToDoContext>(); 
        var controller = new OrderController(loggerMock.Object, dbcontext.Object,  mock.Object); 
        
        //if you call te method right here it will fail because you can't divid by zero
        //
        Action callthemethod = () => controller.DivisionByZero(0);
        

      //  var callthismehtod = controller.DivisionByZero(0);
        
       DivideByZeroException divide  = Assert.Throws<DivideByZeroException>(() => callthemethod());
     //   Assert.Equal("can't divide by zero", divide.Message);
     //   Assert.Equal(2, callthismehtod);
       // Assert.Throws<DivideByZeroException>(() => callthemethod);
   //     Assert.Throws<NullReferenceException>(() => controller.DivisionByZero(numerator));
    Assert.Equal("can't divide by zero", divide.Message);

    //   Assert.Equal<int>(10, callthemethod);
    //   Assert.Equal<int>(909090, callthemethod);

    }
    
    
    
    
  
   [Theory]
   [InlineData("it dosen't matter whats in here")]   //dosen't check if the acutal value is a guid-mock, just hcekcs for the correct type
  // [InlineData(null)]
    public async Task UnitTestGetTotalPrice(string guid)
    {
        
        var mock = new Mock<IRepository<TemporaryCartItems>>();
        var dbocontext = new Mock<ToDoContext>(); 
        var loggerMock = new Mock<ILogger<OrderController>>();
        
           mock.Setup(x => x.ReturnCartItemsByGuidAsync(It.IsAny<string>()))
            .ReturnsAsync(new TemporaryCartItems());
        
        var controller = new OrderController(loggerMock.Object,dbocontext.Object,  mock.Object);
      //  var guid = Guid.NewGuid().ToString();
      
      //this is where you put in the inlinedata
        var results = controller.GetTempsItemsTableByGuid(guid);
      var ok = results.Result as ObjectResult;
      Assert.IsType<TemporaryCartItems>(ok.Value);
     Assert.Equal(200, ok.StatusCode);
    //  Assert.ThrowsAsync<ArgumentNullException>(() => results); 

    }
    
}