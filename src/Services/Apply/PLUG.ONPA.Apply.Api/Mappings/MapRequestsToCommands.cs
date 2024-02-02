using AutoMapper;
using PLUG.ONPA.Apply.Api.Commands;
using PLUG.ONPA.Apply.Api.Requests;

namespace PLUG.ONPA.Apply.Api.Mappings;

public sealed class MapRequestsToCommands : Profile
{
    public MapRequestsToCommands()
    {
        this.CreateMap<SendApplicationRequest, CreateApplicationCommand>()
            .ForCtorParam(nameof(CreateApplicationCommand.TenantId), opt =>
                opt.MapFrom(src => src.TenantId))
            .ForCtorParam(nameof(CreateApplicationCommand.FirstName), opt =>
                opt.MapFrom(src => src.FirstName))
            .ForCtorParam(nameof(CreateApplicationCommand.LastName), opt =>
                opt.MapFrom(src => src.LastName))
            .ForCtorParam(nameof(CreateApplicationCommand.Email), opt =>
                opt.MapFrom(src => src.Email))
            .ForCtorParam(nameof(CreateApplicationCommand.Phone), opt =>
                opt.MapFrom(src => src.Phone))
            .ForCtorParam(nameof(CreateApplicationCommand.AddressCountry), opt =>
                opt.MapFrom(src => src.Address.Country))
            .ForCtorParam(nameof(CreateApplicationCommand.AddressCity), opt =>
                opt.MapFrom(src => src.Address.City))
            .ForCtorParam(nameof(CreateApplicationCommand.AddressState), opt =>
                opt.MapFrom(src => src.Address.State))
            .ForCtorParam(nameof(CreateApplicationCommand.AddressPostalCode), opt =>
                opt.MapFrom(src => src.Address.PostalCode))
            .ForCtorParam(nameof(CreateApplicationCommand.AddressStreet), opt =>
                opt.MapFrom(src => src.Address.Street))
            .ForCtorParam(nameof(CreateApplicationCommand.AddressLine2), opt =>
                opt.MapFrom(src => src.Address.AddressLine2))
            .ForCtorParam(nameof(CreateApplicationCommand.BirthDate), opt =>
                opt.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)))
            .ForCtorParam(nameof(CreateApplicationCommand.ApplicationDate), opt =>
                opt.MapFrom(src => src.ApplicationDate))
            .ForCtorParam(nameof(CreateApplicationCommand.Recommendations), opt =>
                opt.MapFrom(src => src.Recommendations));

        this.CreateMap<AcceptApplicationRequest, AcceptApplicationCommand>()
            .ForCtorParam(nameof(AcceptApplicationCommand.TenantId), opt =>
                opt.MapFrom(src => src.TenantId))
            .ForCtorParam(nameof(AcceptApplicationCommand.ApplicationId), opt =>
                opt.MapFrom(src => src.ApplicationId));

    }
}