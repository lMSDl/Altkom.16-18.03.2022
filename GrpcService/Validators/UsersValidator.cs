using FluentValidation;
using GrpcService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcService.Validators
{
    public class UsersValidator : AbstractValidator<User>
    {
        public UsersValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
