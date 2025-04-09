using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Resturant.Application.Respository;
using Resturant.Domain.Entity;
using Resturant.Infrastructure.Context;

namespace Resturant.Infrastructure.Repository
{
    public class CartItemsRepo : IRepo<CartItems>
    {

        private ToDoContext _context;
        public CartItemsRepo(ToDoContext context)
        {
            _context = context;
        }


        public async Task<List<CartItems>> SearchByName(string name)
        {
            try
            {
                var cartItems = await _context.CartItems.Where(x => x.Name.FirstName == name).ToListAsync();
                return cartItems;
            }
            catch (Exception ex)
            {
              throw new Exception(ex.Message);
            }
        }


        public async Task<List<CartItems>> CartItemsAsync()
        {
            try
            {
                var cartItems = await _context.CartItems.ToListAsync();
                return cartItems;
            }
            catch (Exception ex)
            {
                throw new Exception("why the heck is it not working" + ex.Message);
            }
        }

     
    //THE cartDTO can be used since it will flatten the object 
        public async Task<CartItems> PostItemsAsync(CartItems cart)
        { 
            await  _context.AddAsync(cart);
           await _context.SaveChangesAsync();
           return cart; 
        }

      
    }
}
