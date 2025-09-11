using Microsoft.AspNetCore.Http.HttpResults;
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



    [HttpPost("createuser")]
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
             return Ok(); 
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
        else
        {  
            var emailresult = results?.Email;
            return true; 
        }
    }

    [HttpGet("ReturnAll")]
    public async Task<IActionResult> GetAllUsers()
    {
        var user =  await _userManager.Users.ToListAsync();
        return Ok(user); 
    }

}
