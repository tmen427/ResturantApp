namespace Resturant.Domain.Entity
//same as orderinformation except it does not have List<CartItems>
{ 
    public class UserInformation
    {
        public int Id { get; set; }
        public string? Credit { get; set; }
        public string? NameonCard { get; set; }
        public string? CreditCardNumber { get; set; }
        public string? Expiration { get; set; }

        public string? CVV { get; set; }


        public string? Email { get; set; }

        //required forgein key property 
        public int? UserId { get; set; }

        //the navigation property to the entity
        public User? User { get; set; }

    }
}
