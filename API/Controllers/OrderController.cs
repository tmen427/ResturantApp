
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using API.Repository;
using Resturant.Domain.Entity;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Resturant.Infrastructure.Context;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {

        private readonly IMediator _mediator;

           private readonly ILogger<OrderController> _logger;
        //   private readonly ToDoContext _context; 
        private readonly IRepository<TemporaryCartItems> _temporaryCartRepository;
        
        
        
        

        public OrderController(ILogger<OrderController> logger,  IRepository<TemporaryCartItems> temporaryCartRepository)
        {

            //   _mediator = mediatR ?? throw  new ArgumentNullException(nameof(mediatR));
             _logger = logger ?? throw new ArgumentNullException(nameof(logger));    
            //     _context = context ?? throw new ArgumentNullException(nameof(context)); 
            //   _repo = repo ?? throw new ArgumentNullException("HEY BRO THERES NOT CONSTRO HERE ");

            //no matter whta this will throw since you have registered this service 
            _temporaryCartRepository = temporaryCartRepository ??
                                       throw new ArgumentNullException(nameof(temporaryCartRepository));
        }


        [HttpGet("DivisonByZero")]
        public int DivisionByZero(int numerator)
        {
            if (numerator == 0)
            {
                throw new DivideByZeroException("can't divide by zero");
            }
            return 10/numerator;
        }
        

        public class MenuDTO
        {
            //  public DateTime Date { get; set; }
            public int Id { get; set; }
            public string GuidId { get; set; }
            public string? Name { get; set; }
            public decimal Price { get; set; }
        }


        [HttpGet("TempItemsTable")]
        public async Task<IActionResult> TempCartItems()
        {
            //  var tempCartItems = await _context.TemporaryCartItems.ToListAsync();
            // var tempCartItems = await _context.TemporaryCartItems.Include("MenuItems").ToListAsync();

            var tempCartItems = await _temporaryCartRepository.ReturnListItemsAsync();

            return Ok(tempCartItems);
      //  return tempCartItems;
        }

        [HttpGet("GetTotalPrice")]
        public async Task<IActionResult> GetTempsItemsTableByGuid(string guid)
        {
            //   var tempItemPrice = await _context.TemporaryCartItems.FirstOrDefaultAsync(x=>x.Indentity.ToString() == guid);
            var tempItemPrice = await _temporaryCartRepository.ReturnCartItemsByGuidAsync(guid);
            return Ok(tempItemPrice);
            
        }
        
        //remove these 2
    }
}

//               
//         [HttpGet("GetAllTempItems")]
//         public async Task<ActionResult<List<MenuDTO>>> TemporaryCartItems()
//         {
//
//             var menuDto = await _context.TemporaryCartItems.Include("MenuItems")
//                 .Where(x => x.Indentity.ToString() != string.Empty)
//                 .SelectMany(x => x.MenuItems)
//                 .Select(x => new MenuDTO() { Id = x.Id, Name = x.Name, Price = x.Price, GuidId = x.TemporaryCartItemsIndentity.ToString() }).ToListAsync();
//              
//              var sum = menuDto.Sum(x => x.Price);
//        
//              //send this value to the to the backend somehow 
//              _logger.LogInformation($"Sum: {sum}");
//            
//              
//              if(menuDto.Any()) {
//                 return Ok(menuDto);
//             }
//             return NotFound("no value was found");
//         }
//
//
//         [HttpGet("GetMenuItemByGuid")]
//         public async Task<ActionResult<List<MenuDTO>>> TemporaryCartItemByGuid(
//             [FromQuery] string guidId = "3fa85f64-5717-4562-b3fc-2c963f66afa6")
//         {
//        
//             var guidCheck  = Guid.TryParse(guidId, out var guidValue);
//     
//             if (guidCheck)
//             {
//                 var menuDto = await _context.TemporaryCartItems.Include("MenuItems")
//                     .Where(x => x.Indentity.ToString() == guidValue.ToString())
//                     .SelectMany(x => x.MenuItems)
//                     .Select(x => new MenuDTO()
//                         { Id = x.Id, Name = x.Name, Price = x.Price, GuidId = x.TemporaryCartItemsIndentity.ToString() })
//                     .ToListAsync();
//
//                 if (menuDto.Any())
//                 {
//                     return menuDto;
//                 }
//             }
//             else
//             {
//                 return Ok(0);
//             }
//             //even if nothing is found still return zero
//             int zero = 0; 
//             return Ok(zero); 
//         }
//         
//         
//         [HttpGet("GetMenuItemByGuidSize")]
//         public async Task<ActionResult<int>> TemporaryCartItemByGuidSize(
//             [FromQuery] string guidId = "3fa85f64-5717-4562-b3fc-2c963f66afa6")
//         {
//        
//             var guidCheck  = Guid.TryParse(guidId, out var guidValue);
//     
//             if (guidCheck)
//             {
//                 var menuDto = await _context.TemporaryCartItems.Include("MenuItems")
//                     .Where(x => x.Indentity.ToString() == guidValue.ToString())
//                     .SelectMany(x => x.MenuItems)
//                     .Select(x => new MenuDTO()
//                         { Id = x.Id, Name = x.Name, Price = x.Price, GuidId = x.TemporaryCartItemsIndentity.ToString() })
//                     .ToListAsync();
//
//                 if (menuDto.Any())
//                 {
//                     return Ok(menuDto.Count);
//                 }
//             }
//             else
//             {
//                 return Ok(0);
//             }
//             //even if nothing is found still return zero
//    
//             return Ok(0); 
//         }
//         
//         public class TempDto
//         {
//             public Guid GuidId { get; set; }
//             public string? Name { get; set; }
//         }
//
//
//         [HttpGet("CreateGuide")]
//         public Guid MakeGuid()
//         {
//             var guid = Guid.NewGuid();
//             return guid;
//         }
//
//
//         [HttpDelete("DeleteMenuItem")]
//         //delete menuby
//         public async Task<ActionResult<MenuItemsVO>> ReturnDelete(int id, Guid guidId)
//         {
//            var menuItem =   await _context.MenuItems.FindAsync(id); 
//            if (menuItem != null)
//            {
//                _context.MenuItems.Remove(menuItem);
//            }
//            //save changes needs to be called here
//            await _context.SaveChangesAsync();
//            //delete the item and calculate the new updated price 
//            
//            //update totalprice-AFTER the menu Item has been saved to get the most up to date price
//            var tempCartItem = await _context.TemporaryCartItems.FirstOrDefaultAsync(x => x.Indentity == guidId);
//            
//            var totalPriceMenuItems = 
//                _context.TemporaryCartItems.Include("MenuItems").
//                    Where(x => x.Indentity == guidId).
//                    SelectMany(x=>x.MenuItems).
//                    Sum(x => x.Price);
//            
//             //update price 
//            tempCartItem.TotalPrice = totalPriceMenuItems;
//
//            await _context.SaveChangesAsync();
//            return Ok(menuItem);
//         }
//
//
//         [HttpPost("TemporaryCartItems")]
//
//
//         public async Task<ActionResult<TempDto>> AddTempItems(TempDto dto)
//         {
//             var checkNewMenuItem =
//                 await _context.TemporaryCartItems.SingleOrDefaultAsync(x => x.Indentity == dto.GuidId);
//             
//             //if paid exists means you have already paid
//             var paid = 
//                 await _context.OrderInformation.SingleOrDefaultAsync(x => x.TempCartsIdentity == dto.GuidId);
//             
//             //create a new menu item
//           if (checkNewMenuItem is null && paid is null )
//           {
//       
//               string name = dto.Name!;
//               decimal price = CheckItemPrices(name);
//              
//               TemporaryCartItems temporaryCartItems = new TemporaryCartItems();
//               temporaryCartItems.Indentity = dto.GuidId;
//               temporaryCartItems.Created = DateTime.UtcNow;
//               temporaryCartItems.TotalPrice = price;
//
//               temporaryCartItems.MenuItems.Add(new MenuItemsVO() { Name = name, Price = price });
//               await _context.AddAsync(temporaryCartItems);
//               await _context.SaveChangesAsync();
//             
//              // return CreatedAtAction("TemporaryCartItemByGuid", new {dto.GuidId}, temporaryCartItems);
//              return Ok(new { Name = name, Price = price });
//             // return Ok(temporaryCartItems);
//           }
//    
//           else if (checkNewMenuItem != null && paid is null)
//           {
//               
//               string name = dto.Name!;
//               decimal price = CheckItemPrices(name);
//               
//               var menuItems = new MenuItemsVO() { Name = name, Price = price , TemporaryCartItemsIndentity = dto.GuidId };
//               _context.MenuItems.Add(menuItems);
//              await _context.SaveChangesAsync();
//            
//             //update totalprice-AFTER the menu Item has been saved to get the most up to date price
//               var totalPriceMenuItems = 
//                   _context.TemporaryCartItems.Include("MenuItems").
//                       Where(x => x.Indentity == dto.GuidId).
//                       SelectMany(x=>x.MenuItems).
//                       Sum(x => x.Price);
//
//               checkNewMenuItem.TotalPrice = totalPriceMenuItems;
//               await _context.SaveChangesAsync();
//         
//             //  return CreatedAtAction("TemporaryCartItemByGuid", new {dto.Name}, new {Name = name, Price = price});
//             return Ok(new { Name = name, Price = price });
//           }
//           else
//           {
//               return BadRequest("This user has already paid!!!");
//           }
//
//         }
//         
//         struct ItemPrices
//         {
//             public const decimal TofuStirFry = (decimal) 10.5;
//             public const decimal EggRollPlatter = (decimal) 14.95;
//             public const decimal PapayaSalad = (decimal) 8.95;
//             public const decimal CaesarSalad = (decimal) 8.95;
//             public const decimal ChoppedBeef = (decimal) 12.95;
//             public const decimal VeggiePlatter = (decimal) 8.95;
//         }
//
//         static decimal CheckItemPrices(string itemName)
//         {
//             
//             switch (itemName)
//             {
//                 case "Egg Roll Platter":
//                     return ItemPrices.EggRollPlatter;
//                 case "Papaya Salad":
//                     return ItemPrices.PapayaSalad;
//                 case "Tofu":
//                     return ItemPrices.TofuStirFry;
//                 case "Caesar Salad":
//                     return ItemPrices.CaesarSalad;
//                 case "Chopped Beef":    
//                     return ItemPrices.ChoppedBeef;
//                 case "Veggie Platter":
//                      return ItemPrices.VeggiePlatter;  
//                 default: throw new Exception("That is not a valid item name check " +  nameof(itemName));
//             }
//         }
//     
//         
//     }
// }
