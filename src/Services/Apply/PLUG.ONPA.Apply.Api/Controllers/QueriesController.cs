using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using PLUG.ONPA.Apply.Api.Dtos;
using PLUG.ONPA.Apply.Api.Queries;
using PLUG.ONPA.Apply.Api.Responses;

namespace PLUG.ONPA.Apply.Api.Controllers
{
    [Route("api/apply/")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class QueriesController : ControllerBase
    {
        private readonly IMediator mediator;

        public QueriesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("application/{applicationId}")]
        public async Task<ActionResult<GetApplicationResponse>> GetApplicationById(Guid applicationId)
        {
            var query = new GetApplicationQuery(applicationId);
            var result = await this.mediator.Send(query);
            return result.Match(success =>
            {
                if (success == null)
                {
                    return this.NotFound();
                }
                else
                {
                    var application = new ApplicationDto();
                    var response = new GetApplicationResponse(application);
                    return this.Ok(response);
                }
            }, exception => { return this.BadRequest(exception); });
        }

        [HttpGet("application/{applicationId}/recommendations")]
        public async Task<ActionResult<GetApplicationRecommendationsResponse>> GetApplicationRecommendations(
            Guid applicationId)
        {
            var response = new GetApplicationRecommendationsResponse(new List<RecommendationDto>());
            return this.Ok(response);
        }

        [HttpGet("application/{applicationId}/status")]
        public async Task<ActionResult<GetApplicationStatusResponse>> GetApplicationStatus(Guid applicationId)
        {
            var response = new GetApplicationStatusResponse(new ApplicationStatusDto()
                { Status = 0, Description = "Received" });
            return this.Ok(response);
        }
    }
}