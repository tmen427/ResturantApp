using System.Collections.Immutable;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Restuarant.Application.DTOConversions;
using Resturant.Infrastructure.Context;
using Resturant.Application.DTO;
using Resturant.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : Controller
    {

        private readonly ToDoContext _context;  
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ToDoContext context, ILogger<PaymentController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); 
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<List<OrderInformation>>> GetAllOrders()
        {
            //use a dto to get menu items also 
            var orders = await _context.OrderInformation.ToListAsync();
            if (orders is not null)
            {
                return Ok(orders);
            }
            else return NoContent();
        }

        

        public class ViewsDto
        {
            public required string Name { get; set; } 
          //  public List<MenuItemsVO> MenuItems { get; set; }
            
            public Guid OrderId { get; set; }
            public required List<MenuItemsVO>  Menus { get; set; }
        }
        
        public class MenuDTO1
        {
            public string GuidId { get; set; }
            public string? Name { get; set; }
            public double Price { get; set; }   
        }

        
        
        
        //TODO work on this controller in the near future
        
        [HttpGet("GetAllOrdersWithMenu")]
        public async Task<ActionResult<ViewsDto>> GetAllOrdersWith([FromQuery] string orderGuid = "3fa85f64-5717-4562-b3fc-2c963f66afa6")
        {
            //would it be possible to combine these 2 routes togethor ????
                // var orders =
                //     await _context.OrderInformation.SingleOrDefaultAsync(x =>
                //         x.TempCartsIdentity.ToString() == orderGuid);
                //menuDto will should always return a value-since you can't make an order without menuItems 
            //    var menuDto = await _context.TemporaryCartItems.Include("MenuItems")
              //      .Where(x => x.Indentity.ToString() == orderGuid).ToListAsync();
                   // .SelectMany(x => x.MenuItems).ToListAsync();
                //
    
                var orderList = await _context.OrderInformation.ToListAsync();
                var tempList = await _context.TemporaryCartItems.Include("MenuItems").ToListAsync();
                
              
            //join to the two tables above 
                var joining = from x in orderList
                    join temp in tempList on x.TempCartsIdentity equals temp.Indentity
                    select new ViewsDto() { Name = x.UserName, OrderId = temp.Indentity,  Menus  = temp.MenuItems, };


               var finalValue =  
                   from x in joining 
                   where x.OrderId.ToString() == orderGuid
                   select new { UserName = x.Name, Items = x.Menus.Select(p => p.Name + ", " + p.Price), GuidId = x.OrderId.ToString() }; 
               
                
                if (!finalValue.Any())
                { 
                 return NotFound("the menu item does not exist");
                }
                return Ok(finalValue);
                
        }
        
        
        
        [HttpPost("PaymentInformation")]
        public async Task<ActionResult<OrderInformation>> PostOrderInformation(OrderInformationDTO orderInformation,
            [FromQuery] string orderGuid = "3fa85f64-5717-4562-b3fc-2c963f66afa6")
        {

            //check if the guid exists in any other paying users????/-it shouldn't but just in case 
            var orders = await _context.OrderInformation.FirstOrDefaultAsync(x=>x.TempCartsIdentity.ToString() == orderGuid);
            if (orders is not null)
            {
                //orders should be null, because no other user should have created rhe value yet 
              return BadRequest("the order has already been processed(paid)");
            }
            
            var tempmenuwithguid = await _context.TemporaryCartItems.FirstOrDefaultAsync(x=>x.Indentity.ToString() == orderGuid);
            
            
            var  convertStringToGuid =  Guid.TryParse(orderGuid, out var newGuid);
            //each guid has to be unique! if you have a duplicate guid then 2 users will get the same menuItems
            if (tempmenuwithguid is not null)
            {
                OrderInformation order = new OrderInformation()
                {
                    Credit = orderInformation.Credit,
                    NameonCard = orderInformation.NameonCard,
                    CreditCardNumber = orderInformation.CreditCardNumber,
                    Expiration = orderInformation.Expiration,
                    CVV = orderInformation.CVV,
                    UserName = orderInformation.UserName,
                    TempCartsIdentity = newGuid,
                    Paid = true
                };

                await _context.OrderInformation.AddAsync(order);
                await _context.SaveChangesAsync();

                return Ok(order);
            }
            else
            {
                return BadRequest("There is associated guidId from the menu");
            }

        }
        
  
   [HttpPost("BookingInformation")]
    public async Task<string> BookingInformation(BookingInformation bookingInformation)
    {
        await _context.BookingInformation.AddAsync(bookingInformation);
        await _context.SaveChangesAsync();
        return "the post was succesful";
    }
    
    
    [HttpPost("ContactInformation")]
    public async Task<Contact> PostContactAsync(Contact contact)
    {
            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();
            return contact;
            
    }
    }
}