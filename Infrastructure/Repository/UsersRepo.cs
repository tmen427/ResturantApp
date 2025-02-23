using Microsoft.EntityFrameworkCore;
using Restuarant.Infrastucture.Context;
using Resturant.Domain.Entity;



namespace Resturant.Application.Respository
{
    public class UsersRepo : IRepo<User>
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

    }
}
