using AutoMapper;
using Grpc.Core;
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

        public UsersGrpcService(IUsersService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
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
