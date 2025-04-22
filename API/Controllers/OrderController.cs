
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Resturant.Domain.Entity;

using MediatR;
using Microsoft.EntityFrameworkCore;
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
        private readonly ToDoContext _context; 
        
        public OrderController(IMediator mediatR, ILogger<OrderController> logger, ToDoContext context)
        {

            _mediator = mediatR ?? throw  new ArgumentNullException(nameof(mediatR));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));    
            _context = context ?? throw new ArgumentNullException(nameof(context)); 
        }


        public class MenuDTO
        {
         //  public DateTime Date { get; set; }
            public string GuidId { get; set; }
            public string? Name { get; set; }
            public double Price { get; set; }   
        }


        [HttpGet("TempItemsTable")]
        public async Task<IActionResult> TempCartItems()
        {
           // var tempCartItems = await _context.TemporaryCartItems.Include("MenuItems").ToListAsync();
            var tempCartItems = await _context.TemporaryCartItems.Include("MenuItems").ToListAsync();
            return Ok(tempCartItems);
        }        
              
        [HttpGet("GetAllTempItems")]
        public async Task<ActionResult<List<MenuDTO>>> TemporaryCartItems()
        {

            var menuDto = await _context.TemporaryCartItems.Include("MenuItems")
                .Where(x => x.Indentity.ToString() != string.Empty)
                .SelectMany(x => x.MenuItems)
                .Select(x => new MenuDTO() { Name = x.Name, Price = x.Price, GuidId = x.TemporaryCartItemsIndentity.ToString() }).ToListAsync();
             
             var sum = menuDto.Sum(x => x.Price);
            
             
             //send this value to the to the backend somehow 
             _logger.LogInformation($"Sum: {sum}");
           
             
             if(menuDto.Any()) {
                return Ok(menuDto);
            }
            return NotFound("no value was found");
        }


        [HttpGet("GetMenuItemByGuid")]
        public async Task<ActionResult<List<MenuDTO>>> TemporaryCartItemByGuid(
            [FromQuery] string guidId = "3fa85f64-5717-4562-b3fc-2c963f66afa6")
        {
       
            var guidCheck  = Guid.TryParse(guidId, out var guidValue);
    
            if (guidCheck)
            {
                var menuDto = await _context.TemporaryCartItems.Include("MenuItems")
                    .Where(x => x.Indentity.ToString() == guidValue.ToString())
                    .SelectMany(x => x.MenuItems)
                    .Select(x => new MenuDTO()
                        { Name = x.Name, Price = x.Price, GuidId = x.TemporaryCartItemsIndentity.ToString() })
                    .ToListAsync();

                if (menuDto.Any())
                {
                    return menuDto;
                }
            }
            else
            {
                return NotFound("Not a guid value");
            }

            return NotFound("No items were found");
          
        }




        public class TempDto
        {
            public Guid GuidId { get; set; } = Guid.NewGuid(); 
            public string? Name { get; set; }
        }
        
        
        
        [HttpPost("TemporaryCartItems")]
        public async Task<ActionResult<TempDto>> AddTempItems(TempDto dto)
        {
            var checkNewMenuItem = 
                await  _context.TemporaryCartItems.SingleOrDefaultAsync(x => x.Indentity == dto.GuidId);
            
            //if orderinforamtion exists it means you already payed-or else the user would not have existed
            var paid = await _context.OrderInformation.SingleOrDefaultAsync(x => x.TempCartsIdentity == dto.GuidId);
            
          if (checkNewMenuItem is null && paid is null )
          {
             //update totalprice
              var totalPriceMenuItems = 
                  _context.TemporaryCartItems.Include("MenuItems").
                      Where(x => x.Indentity == dto.GuidId).
                      SelectMany(x=>x.MenuItems).
                      Sum(x => x.Price); 
              
              
              TemporaryCartItems temporaryCartItems = new TemporaryCartItems();
              temporaryCartItems.Indentity = dto.GuidId;
              temporaryCartItems.Created = DateTime.UtcNow;
              temporaryCartItems.TotalPrice = totalPriceMenuItems;
              
              string name = dto.Name;
              double price = CheckItemPrices(name);
              
              temporaryCartItems.MenuItems.Add(new MenuItemsVO() { Name = name, Price = price });
              
              await _context.AddAsync(temporaryCartItems);
              await _context.SaveChangesAsync();
            
              return CreatedAtAction("TemporaryCartItemByGuid", new {dto.GuidId}, temporaryCartItems);
            //  return Ok(temporaryCartItems);
          }
          else if (checkNewMenuItem != null && paid is null)
          {
              
              string name = dto.Name;
              double price = CheckItemPrices(name);
              
              var menuItems = new MenuItemsVO() { Name = name, Price = price , TemporaryCartItemsIndentity = dto.GuidId };
              _context.MenuItems.Add(menuItems);
             await _context.SaveChangesAsync();
           
            //upddte the price-AFTER the menu Item has been saved to get the most up to date price
              var totalPriceMenuItems = 
                  _context.TemporaryCartItems.Include("MenuItems").
                      Where(x => x.Indentity == dto.GuidId).
                      SelectMany(x=>x.MenuItems).
                      Sum(x => x.Price); 
              
              //update total 
              checkNewMenuItem.TotalPrice = totalPriceMenuItems;
              await _context.SaveChangesAsync();
        
              return CreatedAtAction("TemporaryCartItemByGuid", new {dto.Name}, menuItems);
      
          }
          else
          {
              return BadRequest("This user has already paid!!!");
          }

        }
        
        
        struct ItemPrices
        {
            public const double TofuStirFry = 10.5;
            public const double EggRollPlatter = 14.95;
            public const double PapayaSalad = 8.95;
            public const double CaesarSalad = 12.95;
            public const double ChoppedBeef = 12.95;
            public const double VeggiePlatter = 8.95;
        }

        static double CheckItemPrices(string itemName)
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
                default: throw new Exception("That is not a valid item name check " +  nameof(itemName));
            }
        }
    
        
    }
}
