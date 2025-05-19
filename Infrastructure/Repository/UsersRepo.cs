using Microsoft.EntityFrameworkCore;
using Resturant.Application.Respository;
using Resturant.Infrastructure.Context;
using Resturant.Domain.Entity;



namespace Resturant.Infrastructure.Repository
{
    public class UsersRepo : IRepo1<User>
    {
        private readonly ToDoContext _context;
       
        public UsersRepo(ToDoContext context)
        {
            _context = context;
        }
        
        public async Task<List<User>> CartItemsAsync()
        {
            var userList = await _context.Users.ToListAsync();
            return userList;
        }

        public Task<User> PostItemsAsync(User user)
        {
            throw new NotImplementedException();
        }
        
        public async Task<List<User>> SearchByName(string name)
        {
           throw new NotImplementedException(); 
        }
        

    }
}
