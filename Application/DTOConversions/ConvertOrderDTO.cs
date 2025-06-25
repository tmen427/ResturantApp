

using Resturant.Domain.Entity;
using Resturant.Application.DTO; 


namespace Restuarant.Application.DTOConversions
{
    public static class CustomerDTO
    {

      
        public static CustomerInformation ConvertFromOrderDTO(CustomerDto dto)
        {
            return new CustomerInformation()
            {
                Credit = dto.Credit,
                NameonCard = dto.NameonCard,
                CreditCardNumber = dto.CreditCardNumber,
                Expiration = dto.Expiration,
                CVV = dto.CVV,
                UserName = dto.UserName
            };
        }


    }
}

