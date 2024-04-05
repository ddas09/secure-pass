using AutoMapper;
using SecurePass.Data.Entities;

namespace SecurePass.Common.Models.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

        CreateMap<CreateAccountRequestModel, User>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserEmail));
    }
}

