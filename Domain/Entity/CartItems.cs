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

        private CartItems(Guid id, string items, int prices, string name) : base(id)
        {

            item = items;
            price = prices;
            Name = new NameVO(name);


        }

        public string? item { get; private set; }
        public int? price { get; private set; }
        public NameVO? Name { get; private set; }

        public List<Event> Events = new List<Event>();

        public static CartItems CreateCart(Guid id, string items, int prices, string name)
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

            //call save changes in 
        }

        public void ApplyEvents(Event @event)
        {
           switch(@event.Type)
            {
                case(EventItemType.Add):
                    price += @event.Price;   
                    break; 

                case(EventItemType.Decrease):
                    price -= @event.Price; 
                    break; 
                case(EventItemType.Set):
                    price = @event.Price;
                    break; 
            }


        }




    }

}
