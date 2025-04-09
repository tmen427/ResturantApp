
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Resturant.Domain.Entity;
using Resturant.Application.Respository;
using MediatR;
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
      


        public OrderController(IMediator mediatR, ILogger<OrderController> logger)
        {

            _mediator = mediatR ?? throw  new ArgumentNullException(nameof(mediatR));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));    
   
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost("action")]
        public async Task<ActionResult<CartDTO>> PostDto(CartDTO cartDto)
        {
            _logger.LogInformation("Creating order");
            if (ModelState.IsValid)
            {
                _logger.LogInformation("hey the model satte is valid");
                _logger.LogInformation(ModelState.ErrorCount.ToString());
            }

            if (!ModelState.IsValid)
            {
                Console.WriteLine("the MODEL SATE IS INVALID");
            }
     
            PostCartItems items = new(cartDto);
            var databaseItem = await _mediator.Send(items);
            //this has to match one of the routes below  !
            return CreatedAtAction("ReturnCartItemsByName", new { nameoncard = databaseItem.Name}, databaseItem);
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
            GetAllCartItems getallcartitems = new GetAllCartItems(); 
            var listCartItems = await _mediator.Send(getallcartitems);
            return Ok(listCartItems);
        }


        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("ReturnCartItemsByName")]
        public async Task<ActionResult<IEnumerable<CartDTO>>> ReturnCartItemsByName([FromQuery] string nameoncard)
        {
         
            if (string.IsNullOrEmpty(nameoncard))
            {
                return BadRequest(nameof(nameoncard));    
            }

            GetAllCartItemsByName name= new GetAllCartItemsByName(nameoncard);
            var allCartItems = await _mediator.Send(name);
            
            
            if (!allCartItems.Any())
            {
                return NotFound(); 
            }
            return Ok(allCartItems);
        }
        
    }
}
