using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcService.AutoMapper
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<GrpcService.Services.User, Models.User>().ForMember(x => x.BirthDate, x => x.MapFrom(xx => xx.BirthDate.ToDateTime()));
            CreateMap<Models.User, GrpcService.Services.User>().ForMember(x => x.BirthDate, x => x.MapFrom(xx => Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(xx.BirthDate)));
        }
    }
}
