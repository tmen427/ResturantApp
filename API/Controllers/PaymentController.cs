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

        private readonly RestaurantContext _context;  
        private readonly ILogger _logger;

        public PaymentController(RestaurantContext context, ILogger<PaymentController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); 
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<IEnumerable<CustomerInformation>>> GetAllOrders()
        {
            //use a dto to get menu items also 
            var orders = await _context.CustomerInformation.ToListAsync();
            
            if (orders.Any())
            {
                return Ok(orders);
            }
            return BadRequest("No orders were found.");
        }


        public class CustomerInformationdto 
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        
        public class ViewsDto
        {
            public required string Name { get; set; } 
            public Guid OrderId { get; set; }
            public required List<MenuItems>  Menus { get; set; }
        }
        
        
        //TODO work on this controller in the near future-possibly use GroupBy
        [HttpGet("GetAllOrdersWithMenu")]
        public async Task<ActionResult<ViewsDto>> GetAllOrdersWith([FromQuery] string orderGuid = "3fa85f64-5717-4562-b3fc-2c963f66afa6")
        {
            //possibly use group by 
                var orderList = await _context.CustomerInformation.ToListAsync();
                var tempList = await _context.ShoppingCartItems.Include("MenuItems").ToListAsync();
                
              
            //join to the two tables above 
                var joining = from x in orderList
                    join temp in tempList on x.TempCartsIdentity equals temp.Identity
                    select new ViewsDto() { Name = x.UserName, OrderId = temp.Identity,  Menus  = temp.MenuItems, };


               var finalValue =  
                   from x in joining 
                   where x.OrderId.ToString() == orderGuid
                   select new { UserName = x.Name, Items = x.Menus.Select(p => p.Name + ", " + p.Price), GuidId = x.OrderId.ToString() }; 
               
                
                if (!finalValue.Any())
                { 
                 return NotFound("The order does not exist or the order has not ben created yet");
                }
                return Ok(finalValue);
                
        }
        
        
        public class CustomerInformationDTO
        {
            public string? Credit { get; set; }
            public string? NameonCard { get; set; }
            public string? CreditCardNumber { get; set; }
            public string? Expiration { get; set; }
            public string? CVV { get; set; }
            public string? UserName { get; set; }
        
            public bool Paid { get; set; } = false;
            public string CartsIdentity { get; set; }
        
        }




        [HttpPost("PaymentInformation")]
        public async Task<ActionResult<CustomerInformation>> PostOrderInformation(
            CustomerInformationDTO customerInformationDto)
        {

            //we assume all the guids are unique
            var orders = await _context.CustomerInformation.FirstOrDefaultAsync(x =>
                x.TempCartsIdentity.ToString() == customerInformationDto.CartsIdentity);

            if (orders != null)
            {
                return BadRequest("Another pre-existing user has that guid");
            }
            
            var convertGuid = Guid.TryParse(customerInformationDto.CartsIdentity, out var tempGuid);

            _logger.LogCritical(customerInformationDto.CartsIdentity);

            
            //check to make sure the guid must exist in the other table 
            var checkingShoppingCart = await _context.ShoppingCartItems.FirstOrDefaultAsync(x=> x.Identity.ToString() == customerInformationDto.CartsIdentity);
            
            
            
            if (convertGuid && checkingShoppingCart!= null)
            {
                CustomerInformation customer = new CustomerInformation()
                {
                    Credit = customerInformationDto.Credit,
                    NameonCard = customerInformationDto.NameonCard,
                    CreditCardNumber = customerInformationDto.CreditCardNumber,
                    Expiration = customerInformationDto.Expiration,
                    CVV = customerInformationDto.CVV,
                    UserName = customerInformationDto.UserName,
                    TempCartsIdentity = tempGuid,
                    Paid = true
                };

                await _context.CustomerInformation.AddAsync(customer);
                await _context.SaveChangesAsync();

                return Ok(customer);
            }

            return BadRequest("Invalid guid");
        }
    }
    }