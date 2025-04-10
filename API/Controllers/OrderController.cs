
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

        [HttpGet("gettemp")]
        public List<TemporaryCartItems> TemporaryCartItems()
        {
            
        var x =   _context.TemporaryCartItems.Include("MenuItems").Any(x=>x.Indentity.ToString() == "3fa85f64-5717-4562-b3fc-2c963f66afa6");

        var p = _context.TemporaryCartItems.Include("MenuItems")
            .Where(x => x.Indentity.ToString() == "3fa85f64-5717-4562-b3fc-2c963f66afa6").ToList(); 
      //    return   _context.TemporaryCartItems.Include("MenuItems").ToList(); 
      return p; 
        }
        
        

        [HttpPost("temporaryCartItems")]
        public async Task AddTempItems(TemporaryCartItems temporaryCartItems)
        {
            
           await  _context.AddAsync(temporaryCartItems);
           await _context.SaveChangesAsync();
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
