using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using PLUG.ONPA.Apply.Api.Responses;
using PLUG.ONPA.Apply.Api.Responses.Dtos;

namespace PLUG.ONPA.Apply.Api.Controllers
{
    [Route("api/apply/")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class QueriesController : ControllerBase
    {
        [HttpGet("application/{applicationId}")]
        public async Task<ActionResult<GetApplicationResponse>>GetApplicationById(Guid applicationId)
        {
            var application = new ApplicationDto();
            var response = new GetApplicationResponse(application);
            return this.Ok(response);
        }
        
        [HttpGet("application/{applicationId}/recommendations")]
        public async Task<ActionResult<GetApplicationRecommendationsResponse>>GetApplicationRecommendations(Guid applicationId)
        {
            var response = new GetApplicationRecommendationsResponse(new List<RecommendationDto>());
            return this.Ok(response);
        }
        
        [HttpGet("application/{applicationId}/status")]
        public async Task<ActionResult<GetApplicationStatusResponse>>GetApplicationStatus(Guid applicationId)
        {
            var response = new GetApplicationStatusResponse(new ApplicationStatusDto(){Status = 0, Description = "Received"});
            return this.Ok(response);
        }
    }
}
