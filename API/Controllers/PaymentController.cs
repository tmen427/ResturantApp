﻿using System.Collections.Immutable;
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
            else return  BadRequest("No orders were currently found in the database.");
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
        
        
        // [HttpPost("PaymentInformation")]
        // public async Task<ActionResult<CustomerInformation>> PostOrderInformation(CustomerInformation customerInformation)
        // {
        //
        //     //check if the guid for the order exists-it shouldn't but just in case 
        //     var orders = await _context.CustomerInformation.FirstOrDefaultAsync(x=>x.TempCartsIdentity.ToString() == customerInformation.Id);
        //    
        //     if (orders != null)
        //     {
        //         //Idempotent check-no other users should have already paid for the cartItems-if they have this error will popup 
        //       return BadRequest("The order has already been processed-paid");
        //     }
        //     
        //     var cartGuid = await _context.TemporaryCartItems.FirstOrDefaultAsync(x=>x.Indentity.ToString() == customerInformation.GuidId);
        //     _logger.LogInformation(customerInformation.GuidId);
        //     if (cartGuid == null)
        //     {
        //         _logger.LogWarning("this value should not be null"); 
        //     }
        //     
        //     
        //     var  convertStringToGuid =  Guid.TryParse(orderInformation.GuidId, out var newGuid);
        //  
        //     //we assume guid values will always be unique 
        //     if (cartGuid is not null)
        //     { 
        //      
        //         CustomerInformation customer = new CustomerInformation()
        //         {
        //             Credit = orderInformation.Credit,
        //             NameonCard = orderInformation.NameonCard,
        //             CreditCardNumber = orderInformation.CreditCardNumber,
        //             Expiration = orderInformation.Expiration,
        //             CVV = orderInformation.CVV,
        //             UserName = orderInformation.UserName,
        //             TempCartsIdentity = newGuid,
        //             Paid = true
        //         };
        //
        //         await _context.OrderInformation.AddAsync(customer);
        //         await _context.SaveChangesAsync();
        //
        //         return Ok(order);
        //     }
        //     else
        //     {
        //         return BadRequest("There is no ssociated guidId from the menu");
        //     }
        //
        // }
        


    }
}