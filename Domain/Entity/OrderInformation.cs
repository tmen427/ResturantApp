﻿namespace Resturant.Domain.Entity
{
    public class OrderInformation
    {
        public int Id { get; set; }
        public string? Credit { get; set; }
        public string? NameonCard { get; set; }
        public string? CreditCardNumber { get; set; }
        public string? Expiration { get; set; }

        public string? CVV { get; set; }

        //take this out for now
        //public virtual List<CartItems> CartItems { get; set; } = new List<CartItems>();

        public string? UserName { get; set; }
    }
}
