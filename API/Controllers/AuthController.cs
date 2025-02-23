using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using RestuarantBackend.Application.DTO;
//using RestuarantBackend.Infrastructure.Context;
//using RestuarantBackend.Infrastructure.DTO;
//using RestuarantBackend.Infrastructure.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Security.Cryptography;

using Restuarant.Infrastucture.Context; 
using Restuarant.Application.DTO;
using Resturant.Domain.Entity;



namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {

        public User user = new User();
        private readonly ILogger<AuthController> _logger;
        private readonly ToDoContext _toDoContext;
        public IConfiguration Configuration;
        public string JWTTOKEN { get; set; } = null!;


        public AuthController(ILogger<AuthController> logger, ToDoContext toDoContext, IConfiguration configuration)
        {
            _toDoContext = toDoContext;
            Configuration = configuration;
            _logger = logger;
         
        }

        [HttpGet("CheckDuplicateEmail")]
        public async Task<bool> CheckDuplicateEmail(string email)
        {
            var checkusernameexist = await _toDoContext.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            if (checkusernameexist != null)
            {
                //it is a duplicate
                return true;
            }
            //not a duplicate
            return false;
        }

     


        [HttpPost("UserInformation")]

        public async Task<UserInformation> AddUserInformation(UserInformationDTO userInformationDto)
        {
            //get the id of the matching user from the email, the email has to exist in the system 
            var id = await _toDoContext.Users.Where(x => x.Email == userInformationDto.Email).Select(x => x.Id).FirstOrDefaultAsync();
            _logger.LogInformation("the id is ===============" + id);
            //zero means an email was not found, if you try update the given information you cannot yet 
            // var check_userinfo_email = await _toDoContext.Users.Where(x => x.Email == userInformationDto.Email).Include(p => p.UserInformation).Select(x=>x.Id).FirstOrDefaultAsync();

            // _logger.LogInformation("this is the actual value from the query" +  check_userinfo_email.ToString()); 

            //if check_userinfo_email is zero that means that the userinformation for this email does not exist yet 
            if (id != 0)
            {

                UserInformation user = new UserInformation()
                {
                    Credit = userInformationDto.Credit,
                    NameonCard = userInformationDto?.NameonCard,
                    CreditCardNumber = userInformationDto?.CreditCardNumber,
                    Expiration = userInformationDto?.Expiration,
                    CVV = userInformationDto?.CVV,
                    Email = userInformationDto?.Email,
                    UserId = id
                };

                var add = await _toDoContext.UserInformation.AddAsync(user);
                await _toDoContext.SaveChangesAsync();
                return user;
            }
            //return null object if nothing is found
            return new UserInformation()
            {

                Credit = null,
                NameonCard = null,
                CreditCardNumber = null,
                Expiration = null,
                CVV = null,
                Email = null,
                UserId = null

            };
        }

        [HttpPut("UpdateUserInformation")]
        public async Task<UserInformation> UpdateUserInformation([FromQuery]string email, [FromBody] UserInformationDTO userInformationDto)
        {
            //this code works, but how to make it one query, also potentially cache the information 
            var id   = await _toDoContext.UserInformation.Where(x=>x.Email == email).Select(x=>x.Id).FirstOrDefaultAsync();
            _logger.LogInformation("----------------------------");
            var update_certain_entity = _toDoContext.UserInformation.Find(id); 
           
          

            //data for the dto will be preloaded with data from the database, only data that has been updated will be submitted 

            if (update_certain_entity != null)
            {
                if (update_certain_entity.NameonCard != userInformationDto.NameonCard)
                {
                   
                    update_certain_entity.NameonCard = userInformationDto.NameonCard;

                }
                if (update_certain_entity.CVV != userInformationDto.CVV) {
                
                    update_certain_entity.CVV = userInformationDto.CVV;
                }
                if (update_certain_entity.CreditCardNumber != userInformationDto.CreditCardNumber) { 
                     
                    update_certain_entity.CreditCardNumber = userInformationDto.CreditCardNumber;
                }
                if (update_certain_entity.Expiration!= userInformationDto.Expiration)
                {
                    update_certain_entity.Expiration = userInformationDto.Expiration;
                }

                //  var add = _toDoContext.UserInformation.Where(x => x.Id == id).ExecuteUpdate(x => x.SetProperty(x => x.NameonCard, userInformationDto!.NameonCard));
         
                await _toDoContext.SaveChangesAsync();
                return update_certain_entity;
            }
            return null; 
        }
      
    
        


        [HttpGet("GetUserInformation")]
        //based on email???
        public async Task<List<UserInformation>> GetUserInformation()

        {

           
            var userinfo = await _toDoContext.UserInformation.ToListAsync();
            return userinfo;   
        }
       

        //split the search for duplicate string into another method 
        [HttpPost("SignUp")]
        public async Task<Object> SignUp(UserDto userdto)
        {

            var checkusernameexist = _toDoContext.Users.Where(x => x.Email == userdto.Email).FirstOrDefault();

         
            await Console.Out.WriteLineAsync(checkusernameexist?.ToString());
            if (checkusernameexist != null)
            {
                var x = new { error = "duplicate email!" }; 
                var message = System.Text.Json.JsonSerializer.Serialize(x);
                return x; 
            }
            //create a new hashed username and password
            if (checkusernameexist == null && ModelState.IsValid)
            {
                CreatePasswordHash(userdto.Password, out byte[] passwordHash, out byte[] passwordSalt);
                user.Email = userdto.Email;
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt; 

                _toDoContext.Users.Add(user);
                await _toDoContext.SaveChangesAsync();
                return new { success = "the user was added" }; 
            }
            else
            {
                return new { success = "it shouldnt reach here" };  
            }

        }



        [HttpPost("login")]
        public async Task<string> Login(UserDto userdto)
        {

            var profile = await _toDoContext.Users.FirstOrDefaultAsync(x => x.Email == userdto.Email);
            var email = profile?.Email.ToString();

            _logger.LogInformation(email);

            if (email != userdto.Email)
            {
                return "Invalid Email"; 
            }
            //if the new the dto password matches what's in the database then..the password has been varified correctly 
            if (!VerifyPasswordHash(userdto?.Password, profile?.PasswordHash, profile?.PasswordSalt))
            {
                return "Invalid Password"; 
            }

            //return the jwt token if everything is okay 
            JWTTOKEN = ReturnJWTToken(userdto, "Admin");


            return JWTTOKEN;
        }



        // the user can only access it's own information i.e. if the jwt token has that specific email 
        [HttpGet("getusers"), Authorize(Roles = "Admin")]
        //  [HttpGet("getusers")]
        public async Task<User> GetUsers([FromQuery] string email)
        {

            //compare the jwt token to our string email paarameter....and ensure that the values are the same
            //if they are not the same then it is a security loophole
            // get the bearer token back from the authorization token header

            var authorizationheadertoken = HttpContext.Request.Headers["Authorization"].ToString();
         //   await Console.Out.WriteLineAsync("thiis is from the authorization token header");
        //    await Console.Out.WriteLineAsync(authorizationheadertoken);

            var replace = authorizationheadertoken.Replace("bearer ", string.Empty);

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                // ValidIssuer = "your_issuer",
                ValidateAudience = false,
                //  ValidAudience = "your_audience",
                ValidateLifetime = true,
                //    ClockSkew = TimeSpan.Zero
            };

            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(replace, validationParameters, out validatedToken);


        //    await Console.Out.WriteLineAsync(validatedToken.ToString());
            var searchTerm = "email";
            var pos = validatedToken.ToString().IndexOf(searchTerm);
            string temp = validatedToken?.ToString()?.Substring(pos + searchTerm.Length).Trim();
            string[] parts = temp.Split(':');
            string[] comma = parts[1].Split(',');
            string check_email_from_jwtheader = comma[0].Replace("\"", "");
            await Console.Out.WriteLineAsync("the email :" + comma[0].Replace("\"", ""));

            //var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;

            //the email from the jwt token must match the email from which you signed in as
            if (email == check_email_from_jwtheader)
            {
                var allusers = await _toDoContext.Users.Where(x => x.Email == email).Include(s => s.UserInformation).FirstOrDefaultAsync();
                if (allusers.UserInformation?.ToString() == null) { await Console.Out.WriteLineAsync("null users bro "); }
            
                if (allusers.UserInformation is null) await Console.Out.WriteLineAsync("alright the value is simply null");
              
                //if the user does have an associated userinformation it means that have not typed in userinformation in before
                if (allusers.UserInformation != null)
                {
                    return allusers;
                }
                if (allusers.UserInformation is null)
                {
                    return new User()
                    {
                        Id = 0,
                        Email = null,
                        PasswordHash = null,
                        PasswordSalt = null,
                        UserInformation = new UserInformation()
                        {
                            //Id= 1002,
                            Credit = null,
                            NameonCard = null,
                            CreditCardNumber = null,
                            Expiration = null,
                            CVV = null,
                            Email = null,
                            // UserId= 1003
                        }
                    };

                }
            
            }

            //if email does not match with jwt token return null user 
            //return new Models.User
            //{   Id = 0, 
            //    Email = null,
            //    PasswordHash = null,
            //    PasswordSalt = null,

            //};


    
            return new User()
            {
                Id = 0,
                Email = null,
                PasswordHash = null,
                PasswordSalt = null,
                UserInformation = new UserInformation() {
                    //Id= 1002,
                    Credit= null,
                    NameonCard= null,
                    CreditCardNumber= null,
                    Expiration= null,
                    CVV= null,
                    Email= null,
                   // UserId= 1003
                    }
            }; 
        
        

        }




        //the role MUST match the Authorize data annotation 
        private  string ReturnJWTToken(UserDto? user, string? role)
        {
            _logger.LogInformation(user?.Email.ToString());


            List<Claim> claimInfo = new List<Claim>() { new Claim("email", user!.Email.ToString()), 
           // new Claim(ClaimTypes.Role, "Admin")
              new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(claims: claimInfo, expires: DateTime.Now.AddDays(1), signingCredentials: cred);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
          
            
            //send the cookie to the frontend response header 
            //or let the frontend deal with this information
            //HttpContext.Response.Cookies.Append("token", jwt, 
            //    new CookieOptions
            //    {
            //        Expires = DateTime.Now.AddDays(1),
            ////        HttpOnly = true, 
            //        Secure = true,
            //        IsEssential = true,
            //        SameSite = SameSiteMode.None,
            //    }
            //); 

            return jwt;
        }

        private bool VerifyPasswordHash(string? password, byte[]? passwordHash, byte[]? passwordSalt)
        {

            //if the user has a password salt, reingneer the values so that you can compare if the hash goes with the salt 
            using (var HMAC = new HMACSHA512(passwordSalt!))
            {
               
                var computeHash = HMAC.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password!));

                return computeHash.SequenceEqual(passwordHash!);
            }

        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {

            using (var hmac = new HMACSHA512())
            {
          
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }


    }
}
