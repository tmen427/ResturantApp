//using Resturant.Domain.DomainEventHandler;

using Resturant.Domain.DomainEvents;
using Resturant.Domain.EventSourcing;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Resturant.Domain.Entity
{
    public class CartItems : Aggregateroot
    {
        //entity framework needs the empty ctor 
        public CartItems()
        {
        }

        private CartItems(Guid id, List<string> items, double prices, string name) : base(id)
        {

            Item = items;
            Price = prices;
            Name = new NameVO(name);


        }

     //   [Required(ErrorMessage = "Product is required")]
        public List<string> Item  { get; private set; }
        public double Price { get; private set; }
        
       // [Required(ErrorMessage = "The name is required")]
       // [MinLength(2, ErrorMessage = "The name must be at least 2 characters long")]
        public NameVO? Name { get; private set; }

        public List<Event>? Events = new List<Event>();

        public static CartItems CreateCart(Guid id, List<string> items, double prices, string name)
        {
            CartItems cart = new CartItems(id, items, prices, name);
            
            //add event to list<domainevent>
            PriceUpdateDomainEvent update = new PriceUpdateDomainEvent(prices, id);
            cart.RaiseDomainEvent(new PriceUpdateDomainEvent(prices, id));
            
            return cart; 
        }

        public static void RemoveDomainEvents(Guid id, string prices)
        {
            CartItems cart = new();
            // forces you to insnatiate cart, since it's domain removal list<domainevents> is in the aggregate class 
        }

        public void ApplyEvents(Event @event)
        {
           switch(@event.Type)
            {
                case(EventItemType.Add):
                    Price += @event.Price;   
                    break; 

                case(EventItemType.Decrease):
                    Price -= @event.Price; 
                    break; 
                case(EventItemType.Set):
                    Price = @event.Price;
                    break; 
            }


        }




    }

}
