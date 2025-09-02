
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Hangfire.Dashboard;
using Resturant.Domain.Entity;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Resturant.Application.DTO;
using Resturant.Domain.SeedWork;
using Resturant.Infrastructure.Context;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        //private readonly IMediator _mediator;
        private readonly  RestaurantContext _context;
        private readonly IRepository _shoppingCartRepository;
        private readonly ILogger<OrderController> _logger;
        
        
        public OrderController(RestaurantContext context,  IRepository shoppingCartRepository , ILogger<OrderController> logger)
        {
            //   _mediator = mediatR ?? throw  new ArgumentNullException(nameof(mediatR));
            _context = context; 
           _shoppingCartRepository = shoppingCartRepository;
           _logger = logger;
        }


        
        [HttpGet("GetTotalPrice")]
        public async Task<IActionResult> GetShoppingCartItemsByGuid(string guid = "3fa85f64-5717-4562-b3fc-2c963f66afa6")
        {
            
           var shoppingCartItems = await _shoppingCartRepository.ReturnCartItemsByGuidAsync(guid);
           return shoppingCartItems != null ?  Ok(shoppingCartItems) : NotFound();
        }
        
        
         [HttpGet("GetAllTempItems")]
         public async Task<ActionResult<IEnumerable<MenuDTO>>> ShoppingCartItems()
         {
             var menuDtos = (await _shoppingCartRepository.ReturnMenuItemsListAsync())
                 .Select(x => new MenuDTO() { Id = x.Id, Name = x.Name, Price = x.Price});
             return menuDtos.Any() ? Ok(menuDtos) : NotFound("no value was found");
         }


         [HttpGet("GetMenuItemByGuid")]
         public async Task<ActionResult<IEnumerable<MenuDTO>>> ShoppingCartItemByGuid(
             [FromQuery] string guidId = "3fa85f64-5717-4562-b3fc-2c963f66afa6")
         {
             Guid.TryParse(guidId, out var guidIdValue);
             
             // var shoppingCartItems = await _context.ShoppingCartItems.Include("MenuItems").Where(x => x.Identity == guidIdValue).SelectMany(x=>x.MenuItems).ToListAsync();
             // var menus = shoppingCartItems.Select(x => new MenuDTO()
             // {

             //     Id = x.Id,
             //     Name = x.Name,
             //     Price = x.Price,
             //     GuidId = x.ShoppingCartItemsIdentity.ToString()
             // }).ToList(); 
             //
             var menuDto = (await _shoppingCartRepository.ReturnMenuItemListByGuid(guidId.ToString()))
                 .Select(x => new MenuDTO()
                     { Id = x.Id, Name = x.Name, Price = x.Price})
                 .ToList();
             
             return menuDto.Any() ? Ok(menuDto) : NotFound();
         }
         
         [HttpGet("GetMenuItemByGuidSize")]
         public async Task<ActionResult<int>> ShoppingCartItemByGuidSize(
             [FromQuery] string guidId = "3fa85f64-5717-4562-b3fc-2c963f66afa6")
         {
             Guid.TryParse(guidId, out var menudtoItem);

             var menuDto = (await _shoppingCartRepository.ReturnMenuItemListByGuid(menudtoItem.ToString()))
                 .Select(x => new MenuDTO()
                     { Id = x.Id, Name = x.Name, Price = x.Price})
                 .ToList();
   

             int menuDTOSize = menuDto.Count; 
            
             //may not want to return zero if nothing is found-but return Notfound(0) causes error on frontend
             return menuDto.Any() ? Ok(menuDTOSize) : NotFound(0); 
         }
         
         
         [ProducesResponseType(200)]
         [HttpDelete("DeleteMenuItem")]
         public async Task<IActionResult> RemoveMenuItemAndUpdatePrice(int id, Guid guidId)
         {
            var menuItem = await _shoppingCartRepository.FindByPrimaryKey(id); 
            if (menuItem is not null)
            { 
                _context.MenuItems.Remove(menuItem);
                await _shoppingCartRepository.SaveCartItemsAsync();
                //update total price
                var totalPriceMenuItems = _shoppingCartRepository.TotalMenuPrice(guidId); 
                var shoppingCartItems = await _shoppingCartRepository.ReturnCartItemsByGuidAsync(guidId.ToString());
                shoppingCartItems!.TotalPrice = totalPriceMenuItems;
                await _context.SaveChangesAsync();
    
                return Ok(); 
            }
            return NotFound("No menu item was found");
            
         }
         
         
         
         [HttpPost("ShoppingCartItems")]
         public async Task<ActionResult<TempDto>> AddMenuItems(TempDto dto)
         {
             //possibly use single or default
             var shoppingCartItems = await _shoppingCartRepository.ReturnCartItemsByGuidAsync(dto.GuidId.ToString());
    
             
        //     int? customerId = shoppingCartItems?.CustomerInformationId; 
             
             //create a new shopping cart 
           if (shoppingCartItems is null)
           {
               string name = dto.Name!;
               decimal initialprice = CheckingPrices.CheckMenuItemPricesFromStatic(name);
               
               //make a new shopping cart
               ShoppingCartItems shoppingcartitems = new ShoppingCartItems();
               
  
                   shoppingcartitems.Identity = dto.GuidId;
                   shoppingcartitems.Created = DateTime.UtcNow;
                   shoppingcartitems.TotalPrice = initialprice;
                //   shoppingcartitems.CustomerInformationId = null; 
                   
                   var menuItems = new MenuItems();
                   menuItems.Name = name;
                   menuItems.Price = menuItems.CheckMenuItemPrices(name);
                   
                   shoppingcartitems.MenuItems.Add(menuItems);
              
                   
               await _shoppingCartRepository.AddShoppingCartItem(shoppingcartitems);
               await _shoppingCartRepository.SaveCartItemsAsync();
              // return CreatedAtAction("TemporaryCartItemByGuid", new {dto.GuidId}, temporaryCartItems);
              return Ok(new { Name = name, Price = initialprice });
      
           }
           //add new menu items to the shopping cart
           else
           {
               string name = dto.Name!;
               
               var menuItems = new MenuItems();
               menuItems.Name = name;
               menuItems.Price = menuItems.CheckMenuItemPrices(name); 
               
               shoppingCartItems.MenuItems.Add(menuItems);
              await _shoppingCartRepository.SaveCartItemsAsync();
              
             //update totalprice
               var totalPriceMenuItems = _shoppingCartRepository.TotalMenuPrice(dto.GuidId);
                shoppingCartItems.TotalPrice = totalPriceMenuItems;
               await _shoppingCartRepository.SaveCartItemsAsync();
              
             //  return CreatedAtAction("TemporaryCartItemByGuid", new {dto.Name}, new {Name = name, Price = price});
             return Ok(new { Name = name, Price = menuItems.Price });
           }
       
         }
    }
}
