using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Resturant.Domain.Entity;

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
        return Ok(); 

    }

    [HttpPost("Login")]
    public async Task<IActionResult> SignIn(string Username, string Password)
    {
        var result = await _signInManager.PasswordSignInAsync(Username,
            Password, true, lockoutOnFailure: false);
        
        return Ok(result);
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
    public async Task<bool> GetUser(string email)
    {
        var results =  await _userManager.FindByEmailAsync(email);
        if (results == null)
        {
            return false; 
        }
           // var emailresult = results?.Email;
            return true; 
    
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
      var user = await _userManager.GetUserAsync(HttpContext.User);
      return Ok(user);
    }

}
