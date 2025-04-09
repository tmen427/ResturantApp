using System.Runtime.CompilerServices;
using MediatR;
using Resturant.Application.DTO;
using Resturant.Application.Respository;
using Resturant.Domain.Entity;

namespace Resturant.Application.HandleCart.Query;

public class GetAllCartItemByNameHandler : IRequestHandler<GetAllCartItemsByName, List<CartDTO>>
{
    private readonly IRepo<CartItems> _repo;
    
    public GetAllCartItemByNameHandler(IRepo<CartItems> repo)
    {
        _repo = repo;
    }
    
    
    public async Task<List<CartDTO>> Handle(GetAllCartItemsByName request, CancellationToken cancellationToken)
    {
       var cartItems  =  await _repo.SearchByName(request.Name);
      
       var carts = cartItems.Select(item =>
                                     new CartDTO(item.Item, item.Price, item.Name.FirstName)).ToList();
                                     return carts; 

    }
}