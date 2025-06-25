
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
            
            _logger.LogInformation($"Get temps items table by guid: {guid}");
           var shoppingCartItems = await _shoppingCartRepository.ReturnCartItemsByGuidAsync(guid);
            
        //    Guid.TryParse(guid, out Guid shoppingCartGuid);            
        //    var totalPriceShoppingCartItems = _shoppingCartRepository.TotalMenuPrice(shoppingCartGuid); 
             return shoppingCartItems != null ?  Ok(shoppingCartItems) : NotFound();
        }
        
        
         [HttpGet("GetAllTempItems")]
         public async Task<ActionResult<IEnumerable<MenuDTO>>> TemporaryCartItems()
         {
             var menuDTO = (await _shoppingCartRepository.ReturnMenuItemsListAsync())
                 .Select(x => new MenuDTO() { Id = x.Id, Name = x.Name, Price = x.Price, GuidId = x.ShoppingCartItemsIdentity.ToString()});
             return menuDTO.Any() ? Ok(menuDTO) : NotFound("no value was found");
         }


         [HttpGet("GetMenuItemByGuid")]
         public async Task<ActionResult<IEnumerable<MenuDTO>>> TemporaryCartItemByGuid(
             [FromQuery] string guidId = "3fa85f64-5717-4562-b3fc-2c963f66afa6")
         {
             Guid.TryParse(guidId, out var guidIdValue);
             
             var menuDto = (await _shoppingCartRepository.ReturnMenuItemListByGuid(guidId.ToString()))
                 .Select(x => new MenuDTO()
                     { Id = x.Id, Name = x.Name, Price = x.Price, GuidId = x.ShoppingCartItemsIdentity.ToString() })
                 .ToList();
             
             return menuDto.Any() ? Ok(menuDto) : NotFound();
         }
         
         [HttpGet("GetMenuItemByGuidSize")]
         public async Task<ActionResult<int>> TemporaryCartItemByGuidSize(
             [FromQuery] string guidId = "3fa85f64-5717-4562-b3fc-2c963f66afa6")
         {
             Guid.TryParse(guidId, out var menudtoItem);
             
             var menuDto = (await _shoppingCartRepository.ReturnMenuItemListByGuid(menudtoItem.ToString()))
                 .Select(x => new MenuDTO()
                     { Id = x.Id, Name = x.Name, Price = x.Price, GuidId = x.ShoppingCartItemsIdentity.ToString() })
                 .ToList();
            
             return menuDto.Any() ? Ok(menuDto.Count) : NotFound();
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
            var tempCartItem = await _shoppingCartRepository.ReturnCartItemsByGuidAsync(guidId.ToString());
            tempCartItem!.TotalPrice = totalPriceMenuItems;
            await _context.SaveChangesAsync();
            return Ok(); 
         }
         
         //add more items to existing cartItems 
         [HttpPost("TemporaryCartItems")]
         public async Task<ActionResult<TempDto>> AddTempItems(TempDto dto)
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
               decimal price = CheckItemPrices(name);
              
               ShoppingCartItems shoppingcartitems = new ShoppingCartItems();
                   shoppingcartitems.Identity = dto.GuidId;
                   shoppingcartitems.Created = DateTime.UtcNow;
                   shoppingcartitems.TotalPrice = price;
                   shoppingcartitems.MenuItems.Add(new MenuItems() { Name = name, Price = price });
              
               await _context.AddAsync(shoppingcartitems);
               await _context.SaveChangesAsync();
             
              // return CreatedAtAction("TemporaryCartItemByGuid", new {dto.GuidId}, temporaryCartItems);
              return Ok(new { Name = name, Price = price });
      
           }
           else if (checkNewMenuItem != null && paid is null)
           {
               string name = dto.Name!;
               
               //how do we set the price--put this in the actual menuItems class
               //if we refactor this we would just serach the database for menu item pridce
               // await dbcontext.MenuItems.Where(x=x.Name = dto.Name).Select(x=>x.Price).ToDecimal....
               decimal price = CheckItemPrices(name);
               
               //add more items into the cart
               var menuItems = new MenuItems() { Name = name, Price = price , ShoppingCartItemsIdentity = dto.GuidId };
               _context.MenuItems.Add(menuItems);
              await _context.SaveChangesAsync();
            
             //update totalprice-AFTER the menu Item has been saved to get the most up to date price
             //change tracking keeps track of everythiing here--so may only need to call _context.savechanges once??
             
               var totalPriceMenuItems = _shoppingCartRepository.TotalMenuPrice(dto.GuidId);

               checkNewMenuItem.TotalPrice = totalPriceMenuItems;
               await _context.SaveChangesAsync();
               
               
             //  return CreatedAtAction("TemporaryCartItemByGuid", new {dto.Name}, new {Name = name, Price = price});
             return Ok(new { Name = name, Price = price });
           }
           else
           {
               return BadRequest("This user has already paid!!!");
           }

         }
         
         
        
         //REFACTOR INTO menuitems- or just create value object or just 
         struct ItemPrices
         {
             public const decimal TofuStirFry = (decimal) 10.5;
             public const decimal EggRollPlatter = (decimal) 14.95;
             public const decimal PapayaSalad = (decimal) 8.95;
             public const decimal CaesarSalad = (decimal) 8.95;
             public const decimal ChoppedBeef = (decimal) 12.95;
             public const decimal VeggiePlatter = (decimal) 8.95;
         }

         private enum EnumPrices
         {
             TofuStirFry, 
             EggRollPlatter, 
             PapayaSalad,
             CaesarSalad,
             VeggiePlatter,
             
         }
         
         

         static decimal CheckItemPrices(string itemName)
         {
             switch (itemName)
             {
                 case "Egg Roll Platter":
                     return ItemPrices.EggRollPlatter;
                 case "Papaya Salad":
                     return ItemPrices.PapayaSalad;
                 case "Tofu":
                     return ItemPrices.TofuStirFry;
                 case "Caesar Salad":
                     return ItemPrices.CaesarSalad;
                 case "Chopped Beef":    
                     return ItemPrices.ChoppedBeef;
                 case "Veggie Platter":
                      return ItemPrices.VeggiePlatter;  
                 default: throw new InvalidItemException("That is not a valid menu item- " +  itemName);
             }
         }
    
        
    }
}
