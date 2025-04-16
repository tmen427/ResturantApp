namespace Resturant.Domain.Entity
{
    public class OrderInformation
    {
        public int Id { get; set; }
        public string? Credit { get; set; }
        public string? NameonCard { get; set; }
        public string? CreditCardNumber { get; set; }
        public string? Expiration { get; set; }

        public string? CVV { get; set; }
        public string? UserName { get; set; }
        
        public Guid TempCartsIdentity { get; set; }
        
     //   public List<MenuItemsVO>? MenuItems { get; set; } 
        
        //parent to child one to one relationship 
        //whenever there is an order you need to have a cart
       // public TemporaryCartItems? TemporaryCartItems { get; set; }


    }
}
