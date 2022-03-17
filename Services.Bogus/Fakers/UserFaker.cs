using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Bogus.Fakers
{
    public class UserFaker : EntityFaker<User>
    {
        public UserFaker()
        {
            RuleFor(x => x.Login, x => x.Internet.UserName());
            RuleFor(x => x.Password, x => x.Internet.Password());
            RuleFor(x => x.Email, x => x.Internet.Email());
            RuleFor(x => x.Roles, x => x.PickRandom(Enum.GetValues<Roles>(), x.Random.Int(1, 4)).Aggregate((a, b) => a | b));
        }
    }
}
