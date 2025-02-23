using MediatR;
using Resturant.Application.Respository;
using Resturant.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resturant.Application.HandleCart.Command
{
    public class PostCartHandler : IRequestHandler<PostCartItems, CartItems>
    {

        private readonly IRepo<CartItems> _repo; 
        public PostCartHandler(IRepo<CartItems>repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<CartItems> Handle(PostCartItems request, CancellationToken cancellationToken)
        {
            try
            {
                var cartItems = await _repo.PostItemsAsync(request.CartItems);
                return request.CartItems!; 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); 
            }
        }
    }
}
