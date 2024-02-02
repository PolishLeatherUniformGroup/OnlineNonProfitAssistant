using PLUG.ONPA.Apply.Api.Dtos;
using PLUG.ONPA.Common.Application.Dtos;

namespace PLUG.ONPA.Apply.Api.Responses;

public sealed class GetApplicationResponse : SingleResponse<ApplicationDto>
{
    public GetApplicationResponse(ApplicationDto data) : base(data)
    {
    }
}