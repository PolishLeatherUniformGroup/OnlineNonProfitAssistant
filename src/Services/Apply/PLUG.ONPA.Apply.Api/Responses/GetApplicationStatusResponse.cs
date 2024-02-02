using PLUG.ONPA.Apply.Api.Responses.Dtos;
using PLUG.ONPA.Common.Application.Dtos;

namespace PLUG.ONPA.Apply.Api.Responses;

public sealed class GetApplicationStatusResponse : SingleResponse<ApplicationStatusDto>
{
    public GetApplicationStatusResponse(ApplicationStatusDto data) : base(data)
    {
    }
}