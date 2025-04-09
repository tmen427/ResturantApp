using Resturant.Application.DTO;
using MediatR;
namespace Resturant.Application.HandleCart.Query;

public class GetAllCartItemsByName: IRequest<List<CartDTO>>
{
    public string Name { get; set; }
    public GetAllCartItemsByName(string name)
    {
        Name = name;
    }
}