using System.ComponentModel.DataAnnotations;

namespace Resturant.Domain.Entity
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; } = string.Empty;
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }

        //reference navigation property to the dependent entity
        public UserInformation? UserInformation { get; set; }
    }
}
