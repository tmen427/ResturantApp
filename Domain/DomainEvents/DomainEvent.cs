using MediatR;
using Resturant.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resturant.Domain.DomainEvents
{
    public class DomainEvent : IDomainEvent, INotification
    {
        public Guid Id { get; set; }
        
        //empty ctor of ef framework 
        public DomainEvent()
        {
            
        }

        public DomainEvent(Guid id)
        {
            Id = Id; 
        }
    }


//other domain events here .....


    //public sealed class UpdatedPriceDomainEvent : INotification
    //{
    //    public string Price { get; set; }
    //    public Guid Id { get; set; }
    //    public UpdatedPriceDomainEvent(string price,  Guid id)
    //    {
    //        Price = price;
    //        Id = id; 
    //    }

    //}


    }
