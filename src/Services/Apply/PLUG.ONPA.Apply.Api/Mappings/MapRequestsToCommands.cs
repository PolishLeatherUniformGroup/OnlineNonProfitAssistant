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
        
        this.CreateMap<RejectApplicationRequest, RejectApplicationCommand>()
            .ForCtorParam(nameof(RejectApplicationCommand.TenantId), opt =>
                opt.MapFrom(src => src.TenantId))
            .ForCtorParam(nameof(RejectApplicationCommand.ApplicationId), opt =>
                opt.MapFrom(src => src.ApplicationId))
            .ForCtorParam(nameof(RejectApplicationCommand.Reason), opt =>
                opt.MapFrom(src => src.Reason))
            .ForCtorParam(nameof(RejectApplicationCommand.DecisionDate), opt =>
                opt.MapFrom(src => src.RejectionDate));
        
        this.CreateMap<ApproveApplicationRequest, ApproveApplicationCommand>()
            .ForCtorParam(nameof(ApproveApplicationCommand.TenantId), opt =>
                opt.MapFrom(src => src.TenantId))
            .ForCtorParam(nameof(ApproveApplicationCommand.ApplicationId), opt =>
                opt.MapFrom(src => src.ApplicationId))
            .ForCtorParam(nameof(ApproveApplicationCommand.DecisionDate), opt =>
                opt.MapFrom(src => src.ApprovalDate));
        
        this.CreateMap<ApplicationRejectionAppealRequest, AppealApplicationRejectionCommand>()
            .ForCtorParam(nameof(AppealApplicationRejectionCommand.TenantId), opt =>
                opt.MapFrom(src => src.TenantId))
            .ForCtorParam(nameof(AppealApplicationRejectionCommand.ApplicationId), opt =>
                opt.MapFrom(src => src.ApplicationId))
            .ForCtorParam(nameof(AppealApplicationRejectionCommand.Reason), opt =>
                opt.MapFrom(src => src.Reason))
            .ForCtorParam(nameof(AppealApplicationRejectionCommand.AppealDate), opt =>
                opt.MapFrom(src => src.AppealDate));

        this.CreateMap<ApplicationRejectionAppealAcceptRequest, AcceptApplicationRejectionAppealCommand>()
            .ForCtorParam(nameof(AcceptApplicationRejectionAppealCommand.TenantId), opt =>
                opt.MapFrom(src => src.TenantId))
            .ForCtorParam(nameof(AcceptApplicationRejectionAppealCommand.ApplicationId), opt =>
                opt.MapFrom(src => src.ApplicationId));
        
        this.CreateMap<ApplicationRejectionAppealApproveRequest, ApproveApplicationRejectionAppealCommand>()
            .ForCtorParam(nameof(ApproveApplicationRejectionAppealCommand.TenantId), opt =>
                opt.MapFrom(src => src.TenantId))
            .ForCtorParam(nameof(ApproveApplicationRejectionAppealCommand.ApplicationId), opt =>
                opt.MapFrom(src => src.ApplicationId))
            .ForCtorParam(nameof(ApproveApplicationRejectionAppealCommand.Reason), opt =>
                opt.MapFrom(src => src.Reason))
            .ForCtorParam(nameof(ApproveApplicationRejectionAppealCommand.ApproveDate), opt =>
                opt.MapFrom(src => src.ApprovalDate));
        
        this.CreateMap<ApplicationRejectionAppealRejectRequest, RejectApplicationRejectionAppealCommand>()
            .ForCtorParam(nameof(RejectApplicationRejectionAppealCommand.TenantId), opt =>
                opt.MapFrom(src => src.TenantId))
            .ForCtorParam(nameof(RejectApplicationRejectionAppealCommand.ApplicationId), opt =>
                opt.MapFrom(src => src.ApplicationId))
            .ForCtorParam(nameof(RejectApplicationRejectionAppealCommand.Reason), opt =>
                opt.MapFrom(src => src.Reason))
            .ForCtorParam(nameof(RejectApplicationRejectionAppealCommand.RejectDate), opt =>
                opt.MapFrom(src => src.RejectionDate));

        this.CreateMap<ApplicationRejectionAppealDismissRequest, DismissApplicationRejectionAppealCommand>()
            .ForCtorParam(nameof(ApproveApplicationRejectionAppealCommand.TenantId), opt =>
                opt.MapFrom(src => src.TenantId))
            .ForCtorParam(nameof(ApproveApplicationRejectionAppealCommand.ApplicationId), opt =>
                opt.MapFrom(src => src.ApplicationId));
        
        this.CreateMap<ApplicationRecommendationRequest, RequestRecommendationCommand>()
            .ForCtorParam(nameof(RequestRecommendationCommand.TenantId), opt =>
                opt.MapFrom(src => src.TenantId))
            .ForCtorParam(nameof(RequestRecommendationCommand.ApplicationId), opt =>
                opt.MapFrom(src => src.ApplicationId))
            .ForCtorParam(nameof(RequestRecommendationCommand.RequestDate), opt =>
                opt.MapFrom(src => src.RequestDate));
        
        this.CreateMap<ApplicationEndorsedRequest, EndorseApplicationCommand>()
            .ForCtorParam(nameof(EndorseApplicationCommand.TenantId), opt =>
                opt.MapFrom(src => src.TenantId))
            .ForCtorParam(nameof(EndorseApplicationCommand.ApplicationId), opt =>
                opt.MapFrom(src => src.ApplicationId))
            .ForCtorParam(nameof(EndorseApplicationCommand.RecommendationId), opt =>
                opt.MapFrom(src => src.RecommendationId));
        
        this.CreateMap<ApplicationOpposedRequest, OpposeApplicationCommand>()
            .ForCtorParam(nameof(EndorseApplicationCommand.TenantId), opt =>
                opt.MapFrom(src => src.TenantId))
            .ForCtorParam(nameof(EndorseApplicationCommand.ApplicationId), opt =>
                opt.MapFrom(src => src.ApplicationId))
            .ForCtorParam(nameof(EndorseApplicationCommand.RecommendationId), opt =>
                opt.MapFrom(src => src.RecommendationId));
        
        this.CreateMap<ApplicationCancellationRequest, CancelApplicationCommand>()
            .ForCtorParam(nameof(CancelApplicationCommand.TenantId), opt =>
                opt.MapFrom(src => src.TenantId))
            .ForCtorParam(nameof(CancelApplicationCommand.ApplicationId), opt =>
                opt.MapFrom(src => src.ApplicationId))
            .ForCtorParam(nameof(CancelApplicationCommand.CancellationDate), opt =>
                opt.MapFrom(src => src.CancellationDate));
        
        this.CreateMap<DismissApplicationRequest, DismissApplicationCommand>()
            .ForCtorParam(nameof(DismissApplicationCommand.TenantId), opt =>
                opt.MapFrom(src => src.TenantId))
            .ForCtorParam(nameof(DismissApplicationCommand.ApplicationId), opt =>
                opt.MapFrom(src => src.ApplicationId))
            .ForCtorParam(nameof(DismissApplicationCommand.ValidRecommendations), opt =>
                opt.MapFrom(src => src.ValidRecommendations));
        
        this.CreateMap<ApplicationFeeRegistrationRequest,RegisterApplicationFeePaymentCommand>()
            .ForCtorParam(nameof(RegisterApplicationFeePaymentCommand.TenantId), opt =>
                opt.MapFrom(src => src.TenantId))
            .ForCtorParam(nameof(RegisterApplicationFeePaymentCommand.ApplicationId), opt =>
                opt.MapFrom(src => src.ApplicationId))
            .ForCtorParam(nameof(RegisterApplicationFeePaymentCommand.PaymentDate), opt =>
                opt.MapFrom(src => src.PaymentDate))
            .ForCtorParam(nameof(RegisterApplicationFeePaymentCommand.Amount), opt =>
            opt.MapFrom(src => src.FeeAmount))
            .ForCtorParam(nameof(RegisterApplicationFeePaymentCommand.Currency), opt =>
            opt.MapFrom(src => src.FeeCurrency));

    }
}