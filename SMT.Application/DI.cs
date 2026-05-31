
using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SMT.Application.Common.Behaviors;
using SMT.Application.Common.Mappings;
using SMT.Application.Dtos;
using SMT.Application.UserCommand.RegisterUser;
using SMT.Domain.Models;

namespace SMT.Application;

public static class DI
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Регистрация валидаторов FluentValidation
        services.AddValidatorsFromAssembly(assembly);

        // Регистрация MediatR
        services.AddMediatR(assembly);

        // Регистрация AutoMapper с явным указанием профилей
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<AssemblyMappingProfile>();
            
            // Регистрируем профили для record types вручную
            // RegisterUserDto
            cfg.CreateMap<Domain.Models.User, RegisterUserDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ReverseMap();
            
            // YearDTO
            cfg.CreateMap<Year, YearDTO>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.UserId))
                .ForMember(d => d.NameTable, opt => opt.MapFrom(s => s.NameTable))
                .ForMember(d => d.Quarters, opt => opt.MapFrom(s => s.Quarters))
                .ReverseMap();
            
            // QuarterDTO
            cfg.CreateMap<Quarter, QuarterDTO>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Columns, opt => opt.MapFrom(s => s.Columns))
                .ReverseMap();
            
            // MonthColumnDto
            cfg.CreateMap<MonthColumn, MonthColumnDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Month, opt => opt.MapFrom(s => s.Month))
                .ForMember(d => d.Turnover, opt => opt.MapFrom(s => s.Turnover))
                .ForMember(d => d.TaxPayable, opt => opt.MapFrom(s => s.TaxPayable))
                .ForMember(d => d.PaidTax, opt => opt.MapFrom(s => s.PaidTax))
                .ReverseMap();
            
            // UserProfileDto
            cfg.CreateMap<UserProfile, ProfileDTO>().ReverseMap();
        }, assembly);

        // Регистрация ValidationBehavior
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}