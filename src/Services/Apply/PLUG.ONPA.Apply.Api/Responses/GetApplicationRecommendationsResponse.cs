using PLUG.ONPA.Apply.Api.Dtos;
using PLUG.ONPA.Common.Application.Dtos;

namespace PLUG.ONPA.Apply.Api.Responses;

public sealed class GetApplicationRecommendationsResponse : ListResponse<RecommendationDto>
{
    public GetApplicationRecommendationsResponse(ICollection<RecommendationDto> data, int totalCount = 0, int pageSize = 10, int pageNumber = 1) : base(data, totalCount, pageSize, pageNumber)
    {
    }
    
}

