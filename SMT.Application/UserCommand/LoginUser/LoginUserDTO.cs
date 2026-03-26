using System.Globalization;
using AutoMapper;
using SMT.Application.Common.Mappings;

namespace SMT.Application.UserCommand.LoginUser;

public class LoginUserDTO : IMapWith<LoginUserDTO>
{
    public LoginUserDTO() { }
    public string Token { get; set; }
    public string UserName { get; set; }

    public LoginUserDTO(string userName,string token)
    {
        Token = token;
        UserName = userName;
    }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Models.User, LoginUserDTO>().ReverseMap();
    }
    
}