
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Resturant.Domain.Entity;
using Resturant.Application.Respository;
using MediatR;
using Resturant.Application.HandleCart.Query;
using Resturant.Application.HandleCart.Command;
using Resturant.Application.DTO;
using Resturant.Domain.DomainEvents;
using Restuarant.Infrastucture.Context;
using Resturant.Application.DomainEventHandler;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {

        private readonly IMediator _mediator;
        private readonly ILogger<OrderController> _logger;
        private readonly ToDoContext _toDoContext;


        public OrderController(IMediator mediatR, ILogger<OrderController> logger, ToDoContext toDoContext)
        {

            _mediator = mediatR ?? throw  new ArgumentNullException(nameof(mediatR));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));    
            _toDoContext = toDoContext ?? throw new ArgumentNullException(nameof(toDoContext));    
        }

        [ProducesResponseType(200)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost("PostCart")]

        //use a dto instead 
        public async Task<CartItems> PostCart(string items, int prices, string name)
        {

            var newGuid = Guid.NewGuid();

            CartItems cart = CartItems.CreateCart(newGuid, items, prices, name);

            //everytime you create and item here it also adds to an item to the domain event 
            var domainevents =  cart.DomainEvents;

            //this validation won't work if using data annotations
            // Validation.ValidateWithRegex(cartItems.OrderInformationNameonCard); 

            if (ModelState.IsValid)
            {
                PostCartItems postcart = new(cart);
                var databaseItem = await _mediator.Send(postcart);


                //okay this works !
               // PriceUpdateDomainEvent price = new PriceUpdateDomainEvent(prices, Guid.NewGuid());
               // await _mediator.Publish(price); 

                return databaseItem;
            }
            else
            {
                throw new Exception("incorrect model state");
            }
        }

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HttpGet("CartItems")]
        public async Task<List<CartDTO>> GetCartItems()
        {
            GetAllCartItems getallcartItems = new GetAllCartItems();
            return await _mediator.Send(getallcartItems);
        }


        [HttpGet("CartItemsByName")]
        public async Task<IEnumerable<CartDTO>> ReturnCartItemByName(string nameoncard)
        {
            if (string.IsNullOrEmpty(nameoncard))
            {
                throw new NullReferenceException(nameof(nameoncard));
            }

            GetAllCartItems getallcartItems = new GetAllCartItems();
            var allcartItems = await _mediator.Send(getallcartItems);


            IEnumerable<CartDTO> carts = from x in allcartItems
                                         where x.name == nameoncard
                                         select x;


            if (!carts.Any())
            {
                throw new ArgumentException($"no value has been found in {nameof(carts)}");
            }


            return carts;
        }



    }
}
