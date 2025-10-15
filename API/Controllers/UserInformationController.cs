using Azure;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Resturant.Domain.Entity;
using Resturant.Infrastructure.Context;

namespace API.Controllers;
[Route("api/[controller]")]
[ApiController]

public class UserInformationController : Controller
{
    private readonly RestaurantContext _context;
    private readonly ILogger<UserInformationController> _logger;

    public UserInformationController(RestaurantContext context, ILogger<UserInformationController> logger)
    {
        _context = context;
        _logger = logger;

    }


    public class UserPaymentInformationDTO
    {
 
        public string? CardType { get; set; }
        public string? NameonCard { get; set; }
        public string? CreditCardNumber { get; set; }
        public string? Expiration { get; set; }
        public string? CVV { get; set; }
     //   public string?  UserProfileName { get; set; }
    }
    [Authorize]
    [HttpPost("AddUserPaymentInformation")]
    public async Task<ActionResult> AddUserPaymentInformation(UserPaymentInformationDTO userPaymentInformationDTO)
    {
 
        UserPaymentInformation userpaymentinfo = new UserPaymentInformation();
        userpaymentinfo.Credit = userPaymentInformationDTO.CardType;
        userpaymentinfo.NameonCard = userPaymentInformationDTO.NameonCard;
        userpaymentinfo.CreditCardNumber = userPaymentInformationDTO.CreditCardNumber;
        userpaymentinfo.Expiration = userPaymentInformationDTO.Expiration;
        userpaymentinfo.CVV = userPaymentInformationDTO.CVV;
        userpaymentinfo.UserName = HttpContext.User.Identity.Name;
   
   
        await _context.UserPaymentInformation.AddAsync(userpaymentinfo);
        await _context.SaveChangesAsync();
        return Ok(userpaymentinfo);
    }
    
    
    [Authorize]
    [HttpGet("GetUserPaymentInformation")]
    public async Task<IActionResult> GetUserPaymentInformation()
    {
        _logger.LogInformation("Getting the user information below.............");
        //user names cannot be dupicate-if they are the follwing will NOT work
        _logger.LogCritical(HttpContext.User.Identity.Name);

        var user = HttpContext.User.Identity.Name; 
       var userPaymentInformation =   await _context.UserPaymentInformation.OrderBy(x=>x.Id).FirstOrDefaultAsync(x=>x.UserName== user);
     //   var userPaymentInformation =  await _context.UserPaymentInformation.FirstOrDefaultAsync(x=>x.UserName== user);
        return Ok(userPaymentInformation);
    }

    [Authorize]
    [HttpGet("UserHistoryPayments")]
    public async Task<IActionResult> GetUserHistoryPayments()
    {
        var user = HttpContext?.User?.Identity?.Name;
        
        //getting the payment information
        var User = await _context.CustomerPaymentInformation.Where(name => name.UserProfileName == user)
                  .ToListAsync();
     
        //getting the actual car information
               var shoppingCartItems = await _context.ShoppingCartItems.ToListAsync();
                
        
        // //join to the two tables above 
        var joining = from x in User
            join temp in shoppingCartItems on x.ShoppingCartIdentity equals temp.Identity

            //   select new UserHistoryDTO() { UserName =  x.UserProfileName, OrderId = temp.Identity.ToString(),  TotalPrice  = temp.TotalPrice, Date = temp.Created.ToString("d"), };
            select new { TotalPrice = temp.TotalPrice, Date = temp.Created.ToString("d") }; 
        //can do another join here if needed !!!!
        //joing based on guid ida...

        var orderbydate = joining.OrderBy(x => x.Date).Take(5); 
        return Ok(orderbydate); 
    }
    
    

    
    //user mergemap on frontend if needed 
    [Authorize]
    [HttpGet("UserHistoryMenu")]
    public async Task<IActionResult> UserHistoryMenu()
    {
        var user = HttpContext?.User?.Identity?.Name;
            
        //getting the payment information
        var User = await _context.CustomerPaymentInformation.Where(name => name.UserProfileName == user).Distinct()
            .ToListAsync();
        
        //get the guid, from user then jst query 
        var MenuItems = await _context.MenuItems.ToListAsync();
      //  var guidy = await _context.MenuItems.Where(x => x.ShoppingCartItems == Guids).ToListAsync();

        
        // //join to the two tables above 
        var joining =
            from customer in User
            join items in MenuItems on customer.ShoppingCartIdentity equals items.ShoppingCartItemsIdentity
            select new UserPaymentMenuItems()
            {
                Date  = customer.CheckoutTime,
                Identity = customer.ShoppingCartIdentity.ToString(),
                UserProfileName = customer.UserProfileName, 
                MenuItems = new List<MenuItemsBro>() {new MenuItemsBro(){ Date = customer.CheckoutTime.ToString(), Identity = customer.ShoppingCartIdentity.ToString(), Name  = items.Name, Price = items.Price.ToString("0.00") }},
             //  MenuItems = new(){ Identity = customer.ShoppingCartIdentity.ToString(), Name  = items.Name, Price = items.Price.ToString("0.00") }
            };
        
        //flattened the array
        //array within an array 
        var combineall = joining.SelectMany(x => x.MenuItems); 
           //.GroupBy((x => x.Identity));

        //foreach loop here 
        
        //just make a list of items first 
        return Ok(combineall); 

    }
    class UserPaymentMenuItemsClass 
    {
        public DateTime Date { get; set; }
        public string Identity { get; set; }
        public string UserProfileName { get; set; }
        public MenuItemsBro MenuItems  {get; set;}
        
    }
    
    

    class UserPaymentMenuItems
    {
        public DateTime Date { get; set; }
        public string Identity { get; set; }
        public string UserProfileName { get; set; }
        public List<MenuItemsBro>  MenuItems  {get; set;}
        
    }
    
    public class MenuItemsBro
    {
        public string Date { get; set; }
        public string Identity { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
    }
    
    
    
    //find the specific userinfo then update
    [HttpPut("UpdateUserPaymentInformation")]
    public async Task<IActionResult> UpdateUserPaymentInformation(UserPaymentInformationDTO userPaymentInformationDTO)
    {
     
  
        var user = HttpContext.User.Identity.Name; 
        var userPaymentInformation =  await _context.UserPaymentInformation.OrderBy(x=>x.Id).FirstOrDefaultAsync(x=>x.UserName == user);
        
              _logger.LogInformation(userPaymentInformation.NameonCard);
        
        userPaymentInformation.Credit = userPaymentInformationDTO.CardType;
        
        userPaymentInformation.NameonCard = userPaymentInformationDTO.NameonCard;
        userPaymentInformation.CreditCardNumber = userPaymentInformationDTO.CreditCardNumber;
        userPaymentInformation.Expiration = userPaymentInformationDTO.Expiration;
        userPaymentInformation.CVV = userPaymentInformationDTO.CVV;
        
        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("SUCCESFULLLLLL");
        }
        catch (DbUpdateConcurrencyException) 
        {
            return NotFound();
        }

        
        return Ok(userPaymentInformation);

    }

    class UserHistoryDTO
    {
        public string UserName { get; set; }
        public string OrderId { get; set; }
            
        public decimal TotalPrice { get; set; }
        public string Date { get; set; }
    }
    
    
    
}