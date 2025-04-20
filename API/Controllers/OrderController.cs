
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
            public string GuidId { get; set; }
            public string? Name { get; set; }
            public double Price { get; set; }   
        }
        
              
        [HttpGet("GetAllTempItems")]
        public async Task<ActionResult<List<MenuDTO>>> TemporaryCartItems()
        {
          
            var menuDto =  await  _context.TemporaryCartItems.Include("MenuItems")
                .Where(x => x.Indentity.ToString() != string.Empty)
                .SelectMany(x => x.MenuItems)
                .Select(x => new MenuDTO() { Name = x.Name, Price = x.Price, GuidId = x.TemporaryCartItemsIndentity.ToString() }).ToListAsync();
          
            if(menuDto.Any()) {
                return Ok(menuDto);
            }
            return NotFound("no value was found");
        }


        [HttpGet("GetMenuItemByGuid")]
        public async Task<ActionResult<List<MenuDTO>>> TemporaryCartItemByGuid(
            [FromQuery] string GuidId = "3fa85f64-5717-4562-b3fc-2c963f66afa6")
        {


            var menuDto = await _context.TemporaryCartItems.Include("MenuItems")
                .Where(x => x.Indentity.ToString() == GuidId)
                .SelectMany(x => x.MenuItems)
                .Select(x => new MenuDTO()
                    { Name = x.Name, Price = x.Price, GuidId = x.TemporaryCartItemsIndentity.ToString() })
                .ToListAsync();

            if (menuDto.Any())
            {
                return menuDto;
            }
        
            return NotFound("No items were found");
          
        }




        public class TempDto
        {
            //need the guid-from the frontend 
            public Guid GuidId { get; set; } = Guid.NewGuid(); 
            public string Name { get; set; }
        }
        
        
        
        [HttpPost("TemporaryCartItems")]
        public async Task<ActionResult<TempDto>> AddTempItems(TempDto dto)
        {
            var identity = 
                await  _context.TemporaryCartItems.SingleOrDefaultAsync(x => x.Indentity == dto.GuidId);
            
     
              //if orderinforamtion exists it means you already payed-or else the user would not have existed
            var paid = await _context.OrderInformation.SingleOrDefaultAsync(x => x.TempCartsIdentity == dto.GuidId);
            
          if (identity is null && paid is null )
          {
              //mapping 
              TemporaryCartItems temporaryCartItems = new TemporaryCartItems();
              temporaryCartItems.Indentity = dto.GuidId;
              temporaryCartItems.Created = DateTime.UtcNow;
              
              string Name = dto.Name;
              double Price = CheckItemPrices(Name);
              
              temporaryCartItems.MenuItems.Add(new MenuItemsVO() { Name = Name, Price = Price });
              await _context.AddAsync(temporaryCartItems);
              await _context.SaveChangesAsync();
         
              return Ok("A new temporary cart item had been made");
          }
          else if (identity != null && paid is null)
          {
              
              string Name = dto.Name;
              double Price = CheckItemPrices(Name);

              //add to menuItems
              MenuItemsVO tempDto = new MenuItemsVO() { Name = Name, Price = Price , TemporaryCartItemsIndentity = dto.GuidId };
              _context.MenuItems.Add(tempDto);
              await _context.SaveChangesAsync();
              return Ok("Another menu item has been added");
          }
          else
          {
              return BadRequest("the user has already paid!!!");
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

        public static double CheckItemPrices(string ItemName)
        {
            
            switch (ItemName)
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
                default: throw new Exception("That is not a valid item name check " +  nameof(ItemName));
            }
        }
    
        
    }
}
