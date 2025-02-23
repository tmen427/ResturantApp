using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restuarant.Infrastucture.Context;
using Resturant.Domain.Entity;
using Resturant.Domain.EventSourcing;
namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : Controller
    {

        private readonly ToDoContext _toDoContext;
        private readonly ILogger<EventController> _logger;
        public EventController(ToDoContext toDoContext, ILogger<EventController> logger)
        {
            _toDoContext = toDoContext;
            _logger = logger;
        }



        [HttpGet]
        public Task<List<Event>> GetAllEvents()
        {
            return _toDoContext.Events.ToListAsync();
        }

        [HttpPost("PostEvent")]
        public async Task<Event> PostEvent(EventDTO @event)
        {
            try
            {

                Event e = new Event(@event.StreamId, EventItemType.Set, @event.Price); 
                _logger.LogInformation("loggin information here");
                await _toDoContext.Events.AddAsync(e);
                await _toDoContext.SaveChangesAsync();
                return e;
            }
            catch (Exception ex)
            {

                _logger.LogInformation(ex.ToString());
                return null;
            }

        }


        [HttpPost("updateEvent")]

        public async Task UpdateDateEvent(Guid id)
        {
            
      
            var cartItems = await _toDoContext.CartItems.FirstOrDefaultAsync(x => x.Id == id); 
            _logger.LogCritical(cartItems?.Name.FirstName);

              //gets a list of events 
            var events = await _toDoContext.Events.ToListAsync();


            //update the cartitem with each event  
            foreach (Event @event in events)
            {
                _logger.LogInformation(@event.ToString());
                cartItems.ApplyEvents(@event);

            }



            _toDoContext.CartItems.Update(cartItems);
            await _toDoContext.SaveChangesAsync();

        }
        //define some helper methods there 
    }


    public record EventDTO
    {
        public Guid StreamId { get; set; }
   //     public EventItemType Type { get; set; }
        public int Price { get; set; }

        public EventDTO(Guid streamId,  int price)
        {
            StreamId = streamId;
     //       Type = type;
            Price = price;
        }

    }



}
