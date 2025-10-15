using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Resturant.Domain.Entity;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegisterUser : Controller
{
    private readonly UserManager<WebUser> _userManager;
    private readonly SignInManager<WebUser> _signInManager;
    private readonly ILogger<RegisterUser> _logger;


    public RegisterUser(UserManager<WebUser> userManager, SignInManager<WebUser> signInManager,
        ILogger<RegisterUser> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    public class WebUserDTO
    {
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    // [HttpGet("CheckUserLogin")]
    // public bool IsUserCurrentlySignedIn(IdentityUser user)
    // {
    //     // This method can be used to check if a specific user is currently signed in
    //     // (e.g., if you have their IdentityUser object)
    //   //  return _signInManager.IsSignedIn(user);
    // }

    [HttpPost("SignOut")]
    public async Task<IActionResult> SignOut()
    {
        await _signInManager.SignOutAsync();
        return Ok("user is signed out"); 

    }



    public class LoginDTO
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
    
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO login)
    {
                    
         var results = await _userManager.FindByEmailAsync(login.Email);
        
        
        var result = await _signInManager.PasswordSignInAsync(results.UserName,
            login.Password!, true, lockoutOnFailure: false);

      
        _logger.LogCritical(HttpContext.User.Identity.IsAuthenticated.ToString());
        if (result.Succeeded)
        {
              return Ok(result);
        }
        return BadRequest(result);
    }

    


    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser([FromBody] WebUserDTO userDTO)
    {
        //convert here 
        WebUser user = new()
        {
            FullName = userDTO.FullName,
            UserName = userDTO.UserName,
            Email = userDTO.Email,

        };
        //do not allow duplicate emails 
        var results =  await _userManager.FindByEmailAsync(userDTO.Email);
        if (results == null)
        {
            //create new user
         var result = await _userManager.CreateAsync(user, userDTO.Password);
         if (result.Succeeded)
         {
             await _signInManager.SignInAsync(user, true);
             return Ok(userDTO); 
         }
         
         foreach (var error in result.Errors)
         {
             _logger.LogInformation(error.Description);
         }
         return Ok(result); 
        }

        return BadRequest("The email is already in use");
    }


    
    [HttpGet("CheckDuplicateEmail")]
    public async Task<bool> GetUserEmail(string email)
    {
        var results =  await _userManager.FindByEmailAsync(email);
     
        if (results == null)
        {
            return false; 
        }
        return true; 
    
    }

    [HttpGet("FindByUser")]
    public async Task<bool> GetUser(string username)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(username);
            string users = user.UserName;
            return true; 
        }
        catch
        {
            return false; 
        }
    }
    
    

    [HttpGet("ReturnAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {  
        await _userManager.GetUserAsync(HttpContext.User);
        var user =  await _userManager.Users.ToListAsync();
        return Ok(user); 
    }

    
   // [Authorize]
    [HttpGet("GetTheCurrentUser")]
    public async Task<IActionResult> GetTheCurrentUser()
    {
           _logger.LogCritical(HttpContext.User.Identity.Name);
        //    _logger.LogDebug( User.Identity.IsAuthenticated.ToString()); 
        // //   _logger.LogInformation(HttpContext.User.ToString());
        // //   _logger.LogCritical(HttpContext.User.ToString());
        //    _logger.LogCritical(HttpContext.User.Identity.IsAuthenticated.ToString());
      var user = await _userManager.GetUserAsync(HttpContext.User);
  
      if (user != null)
      {
          return Ok(user);
      }
      return Unauthorized();
      //return Ok(false); 
    }

    // [Authorize]
    [HttpGet("IsUserLoggedIn")]
    public async Task<IActionResult> CheckIsUserLoggedIn()
    {
        _logger.LogInformation(HttpContext.User.Identity.Name);
        //    _logger.LogDebug( User.Identity.IsAuthenticated.ToString()); 
        // //   _logger.LogInformation(HttpContext.User.ToString());
        // //   _logger.LogCritical(HttpContext.User.ToString());
        //    _logger.LogCritical(HttpContext.User.Identity.IsAuthenticated.ToString());
        var user = await _userManager.GetUserAsync(HttpContext.User);
  
        if (user != null)
        {
            return Ok(true);
        }
        return Unauthorized();
        //return Ok(false); 
    }

    
    
    
    [Authorize]
    [HttpGet("user")]
    public IActionResult GetUser()
    {
        _logger.LogInformation("User is authenticated");
        return Ok("youur logged in bro ");
    }
    
    
}
