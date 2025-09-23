namespace Resturant.Domain.Entity;

public class UserPaymentInformation
{
    
    public int Id { get; set; }
    public string? Credit { get; set; }
    public string? NameonCard { get; set; }
    public string? CreditCardNumber { get; set; }
    public string? Expiration { get; set; }
    public string? CVV { get; set; }
    public string?  UserProfileName { get; set; }
    
}