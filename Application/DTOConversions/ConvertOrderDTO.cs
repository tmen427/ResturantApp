

using Resturant.Domain.Entity;
using Resturant.Application.DTO; 


namespace Restuarant.Application.DTOConversions
{
    public static class CustomerDTO
    {

      
        public static CustomerPaymentInformation ConvertFromOrderDTO(CustomerDto dto)
        {
            return new CustomerPaymentInformation()
            {
                Credit = dto.Credit,
                NameonCard = dto.NameonCard,
                CreditCardNumber = dto.CreditCardNumber,
                Expiration = dto.Expiration,
                CVV = dto.CVV,
                UserProfileName = dto.UserName
            };
        }


    }
}

