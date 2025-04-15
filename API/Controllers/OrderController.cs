
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Resturant.Domain.Entity;

using MediatR;
using Microsoft.EntityFrameworkCore;

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
        
        
        [HttpGet("getbyGuid")]
        public async Task<List<MenuDTO>> TemporaryCartItemByGuid(string GuidId)
        {
            //only return a specific guid 
            var menuDTO =  await  _context.TemporaryCartItems.Include("MenuItems")
                .Where(x => x.Indentity.ToString() == GuidId)
                .SelectMany(x => x.MenuItems)
                .Select(x => new MenuDTO() { Name = x.Name, Price = x.Price, GuidId = x.TemporaryCartItemsIndentity.ToString() }).ToListAsync();
            
            return menuDTO; 
        }
        
        
        [HttpGet("gettemp")]
        public async Task<List<MenuDTO>> TemporaryCartItems()
        {
            //only return a specific guid 
            var menuDTO =  await  _context.TemporaryCartItems.Include("MenuItems")
                .Where(x => x.Indentity.ToString() != string.Empty)
                .SelectMany(x => x.MenuItems)
                .Select(x => new MenuDTO() { Name = x.Name, Price = x.Price, GuidId = x.TemporaryCartItemsIndentity.ToString() }).ToListAsync();
            
            return menuDTO; 
        }

        public class TempDto
        {
        //    public DateTime Created { get; set; } = DateTime.UtcNow;
            public Guid Id { get; set; } = Guid.NewGuid();
            public string? Name { get; set; }
            public double Price { get; set; }   
        }
        
        
        
        [HttpPost("temporaryCartItems")]
        public async Task<ActionResult<TempDto>> AddTempItems(TempDto dto)
        {
            
            _logger.LogInformation(dto.Id.ToString());
            //check if duplicate keys in database 
          var identity  =  
              _context.TemporaryCartItems.FirstOrDefault(x=>x.Indentity == dto.Id);
            
       //if identity is null, means it is the first time user is adding temporarycratitem
          if (identity is null)
          {
              //mapping 
              TemporaryCartItems temporaryCartItems = new TemporaryCartItems();
              temporaryCartItems.Indentity = dto.Id;
              temporaryCartItems.Created = DateTime.UtcNow;
              temporaryCartItems.MenuItems.Add(new MenuItemsVO() { Name = dto.Name, Price = dto.Price });
              await _context.AddAsync(temporaryCartItems);
              await _context.SaveChangesAsync();
             //possibly add created a route here 
              return Ok("a new temporary cart item had been made");
          }
          else
          {
              _logger.LogInformation("the identity already exists");
              //if it exists already add to existinig 
              MenuItemsVO tempDto = new MenuItemsVO() { Name = dto.Name, Price = dto.Price , TemporaryCartItemsIndentity = dto.Id };
              _context.MenuItems.Add(tempDto);
              await _context.SaveChangesAsync();
              return Ok("Another menu item has been added");
          }

        }
        
        

        public class CartItems
        {
            [Required]
            public string Name { get; set; }
            public double Price { get; set; }   
        }
        
        

        struct ItemPrices
        {
            public const double TofuStirFry = 10.5;
            public const double EggRollPlatter = 10.5;
            public const double PapayaSalad = 10.5;
        }

        public static double CheckItemPrices(string itemname)
        {
            switch (itemname)
            {
                case "Egg Roll Platter":
                    return ItemPrices.EggRollPlatter;
                case "Papaya Salad":
                    return ItemPrices.PapayaSalad;
                case "Tofu Stir Fry":
                    return ItemPrices.TofuStirFry;
                default: return 0; 
            }
        }
    
        
    }
}
