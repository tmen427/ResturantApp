
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Resturant.Domain.Entity;
using Resturant.Application.Respository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Resturant.Application.HandleCart.Query;
using Resturant.Application.HandleCart.Command;
using Resturant.Application.DTO;
using Resturant.Domain.DomainEvents;
using Resturant.Infrastructure.Context;
using Resturant.Application.DomainEventHandler;

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
            public DateTime Created { get; set; } = DateTime.UtcNow;
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
            
       
          if (identity is null)
          {
              //mapping 
              TemporaryCartItems temporaryCartItems = new TemporaryCartItems();
              temporaryCartItems.Indentity = dto.Id;
              temporaryCartItems.Created = dto.Created;
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
              return Ok("another menu item has been added");
          }

        }
        
        
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost("action")]
        public async Task<ActionResult<CartDTO>> PostDto(CartDTO cartDto)
        {
            //store in dctionary righ tnow 
            double total = 0; 
            List<CartItems> cartItems = new List<CartItems>();
            foreach (var item in cartDto.Items)
            {
                cartItems.Add(new  CartItems(){ Name = item, Price = CheckItemPrices(item)});
                total += CheckItemPrices(item);
            }

            foreach (var item in cartItems)
            {
                _logger.LogInformation(item.Price.ToString() + " " + item.Name.ToString());
                _logger.LogInformation(total.ToString());
            }

         
        
            PostCartItems items = new(cartDto);
            var databaseItem = await _mediator.Send(items);
            //this has to match one of the routes below  !
            return CreatedAtAction("ReturnCartItemsByName", new { nameoncard = databaseItem.Name}, databaseItem);
        }

        public class CartItems
        {
            [Required]
            public string Name { get; set; }
            public double Price { get; set; }   
        }
        

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("PostCart")]
        public async Task<ActionResult<CartItems>> PostCart(CartDTO cartDto)
        {
            //this validation won't work if using data annotations
            // Validation.ValidateWithRegex(cartItems.OrderInformationNameonCard); 

            if (ModelState.IsValid)
            {
                _logger.LogInformation($"the model state is correct");
                PostCartItems postcart = new(cartDto);
                var databaseItem = await _mediator.Send(postcart);


                //okay this works !
               // PriceUpdateDomainEvent price = new PriceUpdateDomainEvent(prices, Guid.NewGuid());
               // await _mediator.Publish(price); 

             //   return  Ok(databaseItem);
                return CreatedAtRoute("GetCart", new { Name = databaseItem.Name}, databaseItem);
              //  return CreatedAtAction(databaseItem, postcart); 
            }
            else
            {
                _logger.LogError("CartItems not valid");
                return BadRequest("the model is invalid");
            }
        }

  
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("CartItems")]
        public async Task<ActionResult<List<CartDTO>>> GetCartItems()
        {
            _logger.LogInformation(CheckItemPrices("Egg Roll Platter").ToString());
            GetAllCartItems getallcartitems = new GetAllCartItems(); 
            var listCartItems = await _mediator.Send(getallcartitems);
            return Ok(listCartItems);
        }


        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("ReturnCartItemsByName")]
        public async Task<ActionResult<IEnumerable<CartDTO>>> ReturnCartItemsByName([FromQuery] string nameonCard)
        {
         
            if (string.IsNullOrEmpty(nameonCard))
            {
                return BadRequest(nameof(nameonCard));    
            }

            GetAllCartItemsByName name= new GetAllCartItemsByName(nameonCard);
            var allCartItems = await _mediator.Send(name);
            
            
            if (!allCartItems.Any())
            {
                return NotFound(); 
            }
            return Ok(allCartItems);
        }

        //enums cannot hold 
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
