using Microsoft.AspNetCore.Mvc;
using Restuarant.Application.DTOConversions;
using Restuarant.Infrastucture.Context;
using Resturant.Application.DTO;
using Resturant.Domain.Entity;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : Controller
    {

        public readonly ToDoContext _toDoContext;   

        public PaymentController(ToDoContext toDoContext)
        {
            _toDoContext = toDoContext; 
        }



        [HttpPost("PaymentInformation")]
        public async Task<OrderInformationDTO> PostOrderInformation(OrderInformationDTO order)
        {
            var model = OrderDTO.ConvertFromOrderDTO(order);
            await _toDoContext.OrderInformation.AddAsync(model);
            await _toDoContext.SaveChangesAsync();
            return order;
        }

        [HttpPost("BookingInformation")]
        public async Task<string> BookingInformation(BookingInformation bookingInformation)
        {
            await _toDoContext.BookingInformation.AddAsync(bookingInformation);
            await _toDoContext.SaveChangesAsync();
            return "the post was succesful";
        }


        [HttpPost("ContactInformation")]
        public async Task<Contact> PostContactAsync(Contact contact)
        {
            if (ModelState.IsValid)
            {
                await _toDoContext.Contacts.AddAsync(contact);
                await _toDoContext.SaveChangesAsync();
                return contact;
            }

            return new Contact();
        }
    }
}