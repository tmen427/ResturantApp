using MediatR;
using Resturant.Application.DTO;
using Resturant.Domain.Entity;

namespace Resturant.Application.HandleCart.Command;

public class PostCartItems : IRequest<TempDto>
{
    public TempDto Cartdto { get; set; }    
    public PostCartItems(TempDto cart)
    {
        Cartdto = cart;
    }
}


public class TempDto
{
    //    public DateTime Created { get; set; } = DateTime.UtcNow;
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
    public double Price { get; set; }   
}
