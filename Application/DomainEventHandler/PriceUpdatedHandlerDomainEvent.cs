using MediatR;
using Microsoft.Extensions.Logging;
using Resturant.Domain.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resturant.Application.DomainEventHandler
{
    public  class PriceUpdatedHandlerDomainEvent : INotificationHandler<PriceUpdateDomainEvent>
    {

        private readonly ILogger<PriceUpdatedHandlerDomainEvent> _logger;
        public PriceUpdatedHandlerDomainEvent(ILogger<PriceUpdatedHandlerDomainEvent> logger)
        {
            _logger = logger;
        }


        public async Task Handle(PriceUpdateDomainEvent notification, CancellationToken cancellationToken)
        {

            //THIS should pick iup the events buit  is not !
            _logger.LogInformation("this is finally working i think - from the handlers muaahhh"); 
            _logger.LogInformation($"{notification.Id}  {notification.Price}");
        }
    }
}
