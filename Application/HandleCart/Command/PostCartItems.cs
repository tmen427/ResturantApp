using MediatR;
using Resturant.Application.DTO;

namespace Resturant.Application.HandleCart.Command;

public class PostCartItems : IRequest<CartDTO>
{
    public CartDTO Cartdto { get; set; }    
    public PostCartItems(CartDTO cart)
    {
        Cartdto = cart;
    }
}