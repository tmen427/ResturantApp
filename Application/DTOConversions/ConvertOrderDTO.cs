

using Resturant.Domain.Entity;
using Resturant.Application.DTO; 


namespace Restuarant.Application.DTOConversions
{
    public static class OrderDTO
    {

      
        public static OrderInformation ConvertFromOrderDTO(OrderInformationDTO dto)
        {
            return new OrderInformation()
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

