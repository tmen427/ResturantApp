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

        public readonly ToDoContext _context;  
        public readonly ILogger<PaymentController> _logger;

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
            return  Ok(orders);
        }

        
        [HttpPost("PaymentInformation")]
        public async Task<ActionResult<OrderInformation>> PostOrderInformation(OrderInformationDTO orderInformation,
            [FromQuery] string orderGuid = "3fa85f64-5717-4562-b3fc-2c963f66afa6")
        {
            
            //constrain duplicate usage of orderGuid above 
            var  convertStringToGuid =  Guid.TryParse(orderGuid, out var newGuid);
            OrderInformation order = new OrderInformation()
            {
                Credit = orderInformation.Credit,
                NameonCard = orderInformation.NameonCard,
                CreditCardNumber = orderInformation.CreditCardNumber,
                Expiration = orderInformation.Expiration,
                CVV = orderInformation.CVV,
                UserName = orderInformation.UserName,
                TempCartsIdentity = newGuid,
            };
             
            await _context.OrderInformation.AddAsync(order);
            await _context.SaveChangesAsync();
        
            return Ok(order);
            
        }


       // [HttpGet("ReturnMenuItemByGuid")]
       //  public async Task<TemporaryCartItems> GetAllOrdersByGuid(string orderGuid = "3fa85f64-5717-4562-b3fc-2c963f66afa6")
       //  {
       //
       //  var tempmenuwithguid = await _context.TemporaryCartItems.FirstOrDefaultAsync(x=>x.Indentity.ToString() == orderGuid);
       //         _logger.LogInformation("the value from tempmeuwithguid is" + tempmenuwithguid.OrderInformationId.ToString());
       //         if (String.IsNullOrEmpty(tempmenuwithguid.OrderInformationId.ToString()))
       //         {
       //             //add teh associated order information id...
       //             //will this value be atomic though 
       //             
       //             throw new Exception("wthtthetheh");
       //         }
       //
       //         return tempmenuwithguid;
       //  }
        
        //
    //     [HttpPost("BookingInformation")]
    //     public async Task<string> BookingInformation(BookingInformation bookingInformation)
    //     {
    //         await _context.BookingInformation.AddAsync(bookingInformation);
    //         await _context.SaveChangesAsync();
    //         return "the post was succesful";
    //     }
    //
    //
    //     [HttpPost("ContactInformation")]
    //     public async Task<Contact> PostContactAsync(Contact contact)
    //     {
    //         if (ModelState.IsValid)
    //         {
    //             await _context.Contacts.AddAsync(contact);
    //             await _context.SaveChangesAsync();
    //             return contact;
    //         }
    //
    //         return new Contact();
    //     }
    }
}