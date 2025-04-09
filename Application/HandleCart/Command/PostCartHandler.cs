using MediatR;
using Resturant.Application.Respository;
using Resturant.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resturant.Application.DTO;

namespace Resturant.Application.HandleCart.Command
{
    public class PostCartHandler : IRequestHandler<PostCartItems, CartDTO>
    {

        private readonly IRepo<CartItems> _repo; 
        public PostCartHandler(IRepo<CartItems>repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }
        
        public async Task<CartDTO> Handle(PostCartItems request, CancellationToken cancellationToken)
        {
            try
            { 
                var converttoCartItem = CartItems.CreateCart(Guid.NewGuid(), request.Cartdto.Items, request.Cartdto.Prices, request.Cartdto.Name);
                var cartItems = await _repo.PostItemsAsync(converttoCartItem);
    
                var ConverttoCartDto = new CartDTO(cartItems.Item, cartItems.Price, cartItems.Name.FirstName);
                return ConverttoCartDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); 
            }
        }
    }
}
