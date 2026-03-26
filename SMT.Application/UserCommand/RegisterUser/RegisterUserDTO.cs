using AutoMapper;
using SMT.Domain.Models;
using SMT.Application.Common.Mappings;


namespace SMT.Application.User.RegisterUser;

public class RegisterUserDTO : IMapWith<Domain.Models.User>
{
    public string Name { get; set; }
    public string Password { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Models.User, RegisterUserDTO>().ReverseMap();
    }
}