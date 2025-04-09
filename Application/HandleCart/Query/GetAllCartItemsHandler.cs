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
using Microsoft.Extensions.Logging;

namespace Resturant.Application.HandleCart.Query
{
    public class GetAllCartItemsHandler : IRequestHandler<GetAllCartItems, List<CartDTO>>
    {
        private readonly IRepo<CartItems> _repo;

        public GetAllCartItemsHandler(IRepo<CartItems> repo)
        {
            _repo = repo;
        }
        public async Task<List<CartDTO>> Handle(GetAllCartItems request, CancellationToken cancellationToken)
        {
          var cartItems = await _repo.CartItemsAsync();
          var cartDto = cartItems.Select(items => new CartDTO(items.Item, items.Price, items.Name.FirstName)).ToList();
          return cartDto;
        }
    }
}
