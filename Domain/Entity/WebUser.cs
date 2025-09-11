using Microsoft.AspNetCore.Identity;

namespace Resturant.Domain.Entity;

public class WebUser : IdentityUser
{
    public string? FullName { get; set; }
    

    
}