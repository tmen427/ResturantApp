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
//using OrderController = API.Controllers.OrderController;

namespace OrderControllerTest;

public class UnitTest1
{
    
    [Fact]
    public async void TestOrderControllerTempCartItemsMethod()
    {
        
        List<TemporaryCartItems> temporaryCartItems = new System.Collections.Generic.List<TemporaryCartItems>()
        {
            new TemporaryCartItems() {Id = 1, Indentity = Guid.NewGuid(), Created = DateTime.Now, TotalPrice = 100 },   
            new TemporaryCartItems() {Id = 1, Indentity = Guid.NewGuid(), Created = DateTime.Now, TotalPrice = 100 }, 
            new TemporaryCartItems() {Id = 1, Indentity = Guid.NewGuid(), Created = DateTime.Now, TotalPrice = 100 }, 
        };
  
   var mockRepo = new Mock<IRepository<TemporaryCartItems>>();
   mockRepo.Setup(m => m.ReturnListItemsAsync()).ReturnsAsync(temporaryCartItems);


     // var controller = new OrderController(dbContext.Object,  mockRepo.Object);
     var controller = new OrderController(null, mockRepo.Object);
      var results = await controller.TempCartItems();
      var okResult = results as OkObjectResult;


      Assert.IsAssignableFrom<IActionResult>(okResult);
      Assert.Equal(200, okResult.StatusCode);
    }

    
    
    [Fact]
    public async Task UnitTestGetTotalPrice()
    {
        
        var mock = new Mock<IRepository<TemporaryCartItems>>();
        var dbocontext = new Mock<ToDoContext>(); 

        
           mock.Setup(x => x.ReturnCartItemsByGuidAsync(It.IsAny<string>()))
            .ReturnsAsync(new TemporaryCartItems());
        
        var controller = new OrderController(null,  mock.Object);
        var guid = Guid.NewGuid().ToString();
        var results = controller.GetTempsItemsTableByGuid(guid);
        var ok = results.Result as ObjectResult;
        Assert.IsType<TemporaryCartItems>(ok.Value);
        Assert.Equal(200, ok.StatusCode);
    //  Assert.ThrowsAsync<ArgumentNullException>(() => results); 

    }
  

  [Fact]
  public void  UnitTestingDeleteMethod()
  {
      
      var mockSet = new Mock<DbSet<TemporaryCartItems>>();
      
      //  var mockOptions = new Mock<DbContextOptions<ToDoContext>>();
        var mockDbContext = new Mock<ToDoContext>(); 
       
        //mockDbContext.Setup(m=>m.Set<TemporaryCartItems>()).Returns(mockSet.Object);
        OrderController orderController = new OrderController(mockDbContext.Object, null);
        var ok = orderController.ReturnDelete(1, Guid.Empty);
        
        Assert.IsAssignableFrom<Task<ActionResult<MenuItemsVO>>>(ok);
      
   

  }
  
  
  
    
}