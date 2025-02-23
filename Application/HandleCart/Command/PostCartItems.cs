using MediatR;
using Resturant.Application.DTO;
using Resturant.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resturant.Application.HandleCart.Command
{
    public class PostCartItems : IRequest<CartItems>
    {
        public CartItems CartItems { get; set; }

        public PostCartItems(CartItems cartItems)
        {
            CartItems = cartItems;
        }

    }
}
