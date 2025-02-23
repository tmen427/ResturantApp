
using Microsoft.EntityFrameworkCore;
using Resturant.Domain.Entity;
using Microsoft.Extensions.Logging;
using Restuarant.Infrastucture.Context;
using Resturant.Application.DTO;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace Resturant.Application.Respository
{
    public class CartItemsRepo : IRepo<CartItems>
    {
        private ILogger<CartItemsRepo> _logger;
        private ToDoContext _context;
       
        public CartItemsRepo(ToDoContext context, ILogger<CartItemsRepo> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<List<CartItems>> CartItemsAsync()
        {
            var cartItems = await _context.Set<CartItems>().AsNoTracking<CartItems>().ToListAsync();    
            return cartItems; 
        }

     
    //THE cartDTO can be used since it will flatten the object 
        public async Task<CartItems> PostItemsAsync(CartItems cart)
        { 

           await _context.Set<CartItems>().AddAsync(cart);
           await _context.SaveChangesAsync();


    
           return cart; 
        }

      
    }
}
