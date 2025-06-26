
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        

        [HttpGet("TempItemsTable")] 
        public async Task<IActionResult> TempCartItems()
        {
            var shoppingCartItems = await _shoppingCartRepository.ReturnListItemsAsync(); 
            return shoppingCartItems.Count == 0 ? NotFound() : Ok(shoppingCartItems);
        }

        
        [HttpGet("GetTotalPrice")]
        public async Task<IActionResult> GetTempsItemsTableByGuid(string guid = "3fa85f64-5717-4562-b3fc-2c963f66afa6")
        {
           var shoppingCartItems = await _shoppingCartRepository.ReturnCartItemsByGuidAsync(guid);
           return shoppingCartItems != null ?  Ok(shoppingCartItems) : NotFound();
        }
        
        
         [HttpGet("GetAllTempItems")]
         public async Task<ActionResult<IEnumerable<MenuDTO>>> ShoppingCartItems()
         {
             var menuDTO = (await _shoppingCartRepository.ReturnMenuItemsListAsync())
                 .Select(x => new MenuDTO() { Id = x.Id, Name = x.Name, Price = x.Price});
             return menuDTO.Any() ? Ok(menuDTO) : NotFound("no value was found");
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
            
             return menuDto.Any() ? Ok(menuDTOSize) : NotFound();
         }
         

         
         [ProducesResponseType(200)]
         [HttpDelete("DeleteMenuItem")]
         public async Task<IActionResult> RemoveMenuItemAndUpdatePrice(int id, Guid guidId)
         {
            var menuItem = await _shoppingCartRepository.FindByPrimaryKey(id); 
            if (menuItem is not null)
            {
                _context.MenuItems.Remove(menuItem);
            }
            await _shoppingCartRepository.SaveCartItemsAsync();
            
            //update total price
            var totalPriceMenuItems = _shoppingCartRepository.TotalMenuPrice(guidId); 
            var shoppingCartItems = await _shoppingCartRepository.ReturnCartItemsByGuidAsync(guidId.ToString());
            shoppingCartItems!.TotalPrice = totalPriceMenuItems;
            await _context.SaveChangesAsync();
            return Ok(); 
         }
         
         
         
         [HttpPost("ShoppingCartItems")]
         public async Task<ActionResult<TempDto>> AddMenuItems(TempDto dto)
         {
             var checkNewMenuItem =
                 await _context.ShoppingCartItems.SingleOrDefaultAsync(x => x.Identity == dto.GuidId);
             
             //if paid exists means you have already paid-should not paid be at this point
             var paid = 
                 await _context.CustomerInformation.SingleOrDefaultAsync(x => x.TempCartsIdentity == dto.GuidId);
             
             //create a new menu item
           if (checkNewMenuItem is null && paid is null )
           {
               string name = dto.Name!;
               
               
               //possibly refactor this 
               decimal price = CheckingPrices.CheckMenuItemPricesFromStatic(name);
               
               ShoppingCartItems shoppingcartitems = new ShoppingCartItems();
                   shoppingcartitems.Identity = dto.GuidId;
                   shoppingcartitems.Created = DateTime.UtcNow;
                   shoppingcartitems.TotalPrice = price;
                   
                   var menuItems = new MenuItems();
                   menuItems.Name = name;
                   menuItems.Price = menuItems.CheckMenuItemPrices(name);
                   shoppingcartitems.MenuItems.Add(menuItems);
              
               await _context.AddAsync(shoppingcartitems);
               await _context.SaveChangesAsync();
             
              // return CreatedAtAction("TemporaryCartItemByGuid", new {dto.GuidId}, temporaryCartItems);
              return Ok(new { Name = name, Price = price });
      
           }
           else if (checkNewMenuItem != null && paid is null)
           {
               string name = dto.Name!;
               
               var menuItems = new MenuItems();
               menuItems.Name = name;
               menuItems.Price = menuItems.CheckMenuItemPrices(name); 
               
              checkNewMenuItem.MenuItems.Add(menuItems);
              await _shoppingCartRepository.SaveCartItemsAsync();
              
             //update totalprice
               var totalPriceMenuItems = _shoppingCartRepository.TotalMenuPrice(dto.GuidId);
               checkNewMenuItem.TotalPrice = totalPriceMenuItems;
              await _shoppingCartRepository.SaveCartItemsAsync();
               
               
             //  return CreatedAtAction("TemporaryCartItemByGuid", new {dto.Name}, new {Name = name, Price = price});
             return Ok(new { Name = name, Price = menuItems.Price });
           }
           else
           {
               return BadRequest("This user has already paid!!!");
           }
         }
    }
}
