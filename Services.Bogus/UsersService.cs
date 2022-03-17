using Models;
using Services.Bogus.Fakers;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Bogus
{
    public class UsersService : CrudService<User>, IUsersService
    {
        public UsersService(EntityFaker<User> faker) : base(faker)
        {
            CreateAsync(new User() { Login = "Admin", Password = "@dmin", Roles = Roles.Create | Roles.Update | Roles.Read });
        }

        public Task<User> ReadByLoginAsync(string login)
        {
            return Task.FromResult(_entities.SingleOrDefault(x => x.Login == login));
        }

        public async Task<string> ResetPasswordAsync(int id)
        {
            var user = await ReadAsync(id);
            if (user == null)
                return null;

            user.Password = Guid.NewGuid().ToString();
            return user.Password;
        }
    }
}
