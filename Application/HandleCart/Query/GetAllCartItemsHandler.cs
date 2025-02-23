using MediatR;
using Resturant.Application.DTO;
using Resturant.Application.Respository;
using Resturant.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Resturant.Application.HandleCart.Query
{
    public class GetAllCartItemsHandler : IRequestHandler<GetAllCartItems, List<CartDTO>>
    {

        //readonly values can only be set within the ctor or upon instantiation of this object...
        private readonly IRepo<CartItems> _repo;
        public GetAllCartItemsHandler(IRepo<CartItems> repo)
        {
            _repo = repo;
        }

        public async Task<List<CartDTO>> Handle(GetAllCartItems request, CancellationToken cancellationToken)
        {
            List<CartDTO> cartDTO = new();  
            
           // return await _repo.CartItemsAsync();
            //convert to dto 
            List<CartItems> cartItems = await _repo.CartItemsAsync();

          //  artDTO(int id, string items, string prices, string name)

            foreach (var cart in cartItems )
            {
                CartDTO carts = new CartDTO(cart.Id, cart.item!, cart.price, cart.Name?.FirstName!); 
                cartDTO.Add(carts);
            }

            return cartDTO; 

   
        }
    }
}
