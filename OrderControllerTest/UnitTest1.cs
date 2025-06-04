using System.Net;
using System.Runtime.CompilerServices;
using API.Controllers;
using API.Repository;
using FluentAssertions.Specialized;

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


    public List<TemporaryCartItems> ReturnListOfTemporaryCartItems()
    {
        List<TemporaryCartItems> temporaryCartItems = new System.Collections.Generic.List<TemporaryCartItems>()
        {
            new TemporaryCartItems() { Id = 1, Indentity = Guid.NewGuid(), Created = DateTime.Now, TotalPrice = 100 },
            new TemporaryCartItems() { Id = 1, Indentity = Guid.NewGuid(), Created = DateTime.Now, TotalPrice = 100 },
            new TemporaryCartItems() { Id = 1, Indentity = Guid.NewGuid(), Created = DateTime.Now, TotalPrice = 100 },
        };
        
        return temporaryCartItems;
        
    }
    
    

    [Fact]
    public async void TempCartItemsTableReturnsOk()
    {

        List<TemporaryCartItems> temporaryCartItems = new System.Collections.Generic.List<TemporaryCartItems>()
        {
            new TemporaryCartItems() { Id = 1, Indentity = Guid.NewGuid(), Created = DateTime.Now, TotalPrice = 100 },
            new TemporaryCartItems() { Id = 1, Indentity = Guid.NewGuid(), Created = DateTime.Now, TotalPrice = 100 },
            new TemporaryCartItems() { Id = 1, Indentity = Guid.NewGuid(), Created = DateTime.Now, TotalPrice = 100 },
        };

        var mockRepo = new Mock<IRepository>();
        mockRepo.Setup(m => m.ReturnListItemsAsync()).ReturnsAsync(temporaryCartItems);
        
        var controller = new OrderController(null, mockRepo.Object);
        var results = await controller.TempCartItems();
        var okResult = results as OkObjectResult;
        
        Assert.IsAssignableFrom<IActionResult>(okResult);
        Assert.Equal(200, okResult?.StatusCode);
    }



    [Fact]
    public async Task GetTotalPriceReturnsTotalPrice()
    {

        var mock = new Mock<IRepository>();
        var dbocontext = new Mock<ToDoContext>();
        
        mock.Setup(x => x.ReturnCartItemsByGuidAsync(It.IsAny<string>()))
            .ReturnsAsync(new TemporaryCartItems());

        var controller = new OrderController(dbocontext.Object, mock.Object);
        var guid = Guid.NewGuid().ToString();
        var results = controller.GetTempsItemsTableByGuid(guid);
        var ok = results.Result as ObjectResult;
        Assert.IsType<TemporaryCartItems>(ok.Value);
        Assert.Equal(200, ok.StatusCode);
        //  Assert.ThrowsAsync<ArgumentNullException>(() => results); 

    }


    [Fact]
    public void DeleteMethodshouldreturnMenutItems()
    {
        var mockSet = new Mock<DbSet<TemporaryCartItems>>();
        var mockDbContext = new Mock<ToDoContext>();

        //  mockDbContext.Setup(m=>m.Remove(It.IsAny<TemporaryCartItems>())).Verifiable();
          mockDbContext.Setup(m => m.SaveChanges()).Returns(1);

        //mockDbContext.Setup(m=>m.Set<TemporaryCartItems>()).Returns(mockSet.Object);
        OrderController orderController = new OrderController(mockDbContext.Object, null);
        var ok = orderController.RemoveMenuItem(1, Guid.Empty);

        Assert.IsAssignableFrom<Task<ActionResult<MenuItemsVO>>>(ok);

    }

    [Fact]
    public async Task RemoveTempCartItemsMethodAsyncTask()
    {
        var mockRepo = new Mock<IRepository>();
        var menuItems= new MenuItemsVO() { Name = "Egg Rolls" };
        
        mockRepo.Setup(m => m.FindByPrimaryKey(It.IsAny<int>())).ReturnsAsync(menuItems);
        mockRepo.Setup(m => m.SaveCartItemsAsync()).ReturnsAsync(0);
 
        OrderController orderController = new OrderController(null, mockRepo.Object);
        var result = orderController.RemoveMenuItem(1, Guid.Empty);
        
        Assert.IsAssignableFrom<Task<ActionResult<MenuItemsVO>>>(result);
        
    }


  
}