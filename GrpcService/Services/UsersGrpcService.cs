using AutoMapper;
using FluentValidation;
using Grpc.Core;
using GrpcService.Validators;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcService.Services
{
    public class UsersGrpcService : GrpcUsers.GrpcUsersBase
    {
        private IUsersService _service;
        private IMapper _mapper;
        private IValidator<User> _validator; 

        public UsersGrpcService(IUsersService service, IMapper mapper, IValidator<User> validator)
        {
            _service = service;
            _mapper = mapper;
            _validator = validator;
        }

        public override async Task<User> Create(User request, ServerCallContext context)
        {
            var result = await _service.CreateAsync(_mapper.Map<Models.User>(request));

            return _mapper.Map<User>(result);
        }

        public override async Task<None> Delete(User request, ServerCallContext context)
        {
            await _service.DeleteAsync(request.Id);
            return new None();
        }

        public override async Task<Users> Read(None request, ServerCallContext context)
        {
            var users = await _service.ReadAsync();
            var response = new Users();
                response.Collection.AddRange(users.Select(x =>
                {
                    x.BirthDate = x.BirthDate.ToUniversalTime();
                    return _mapper.Map<User>(x);
                }));
            return response;
        }

        public override async Task<User> ReadById(User request, ServerCallContext context)
        {
            if (!_validator.Validate(request).IsValid)
                return new User();

            var user = await _service.ReadAsync(request.Id);
            return _mapper.Map<User>(user);
        }

        public override async Task<None> Update(User request, ServerCallContext context)
        {
            await _service.UpdateAsync(request.Id, _mapper.Map<Models.User>(request));

            return new None();
        }
    }
}
