using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resturant.Domain.DomainEvents
{
    public class PriceUpdateDomainEvent :INotification
    {
       public int Price {  get; set; }   
       public Guid Id { get; set; }

        public PriceUpdateDomainEvent(int price, Guid id)
        {
            Price = price;
            Id = id;
        }
    }
}
