using PLUG.ONPA.Apply.Api.Dtos;

namespace PLUG.ONPA.Apply.Api.Requests;

public sealed record SendApplicationRequest(
    Guid? TenantId,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    AddressDto Address,
    DateTime BirthDate,
    DateTime ApplicationDate,
    List<string> Recommendations);

public sealed record AcceptApplicationRequest(Guid ApplicationId, Guid? TenantId);
public sealed record DismissApplicationRequest(Guid ApplicationId, Guid? TenantId, List<string> ValidRecommendations);
public sealed record ApproveApplicationRequest(Guid ApplicationId, Guid? TenantId, DateTime ApprovalDate);
public sealed record RejectApplicationRequest(Guid ApplicationId, Guid? TenantId, DateTime RejectionDate, string Reason);
public sealed record ApplicationRejectionAppealRequest(Guid ApplicationId, Guid? TenantId, DateTime AppealDate, string Reason);
public sealed record ApplicationRejectionAppealDismissRequest(Guid ApplicationId, Guid? TenantId);
public sealed record ApplicationRejectionAppealAcceptRequest(Guid ApplicationId, Guid? TenantId);
public sealed record ApplicationRejectionAppealApproveRequest(Guid ApplicationId, Guid? TenantId, DateTime ApprovalDate, string Reason);
public sealed record ApplicationRejectionAppealRejectRequest(Guid ApplicationId, Guid? TenantId, DateTime RejectionDate, string Reason);
public sealed record ApplicationRecommendationRequest(Guid ApplicationId, Guid? TenantId, DateTime RequestDate);
public sealed record ApplicationCancellationRequest(Guid ApplicationId, Guid? TenantId, DateTime CancellationDate);
public sealed record ApplicationEndorsedRequest(Guid ApplicationId, Guid? TenantId, Guid RecommendationId);
public sealed record ApplicationOpposedRequest(Guid ApplicationId, Guid? TenantId, Guid RecommendationId);
public sealed record ApplicationFeeRegistrationRequest(Guid ApplicationId, Guid? TenantId, decimal FeeAmount, string FeeCurrency, DateTime PaymentDate);
