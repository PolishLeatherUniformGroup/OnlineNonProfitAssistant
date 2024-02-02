using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PLUG.ONPA.Apply.Api.Requests;

namespace PLUG.ONPA.Apply.Api.Controllers
{
    [Route("api/apply/commands")]
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    public class CommandsController : ControllerBase
    {
        private readonly IMediator mediator;

        public CommandsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("send-application")]
        public async Task<IActionResult> CreateApplicationForm([FromBody]SendApplicationRequest request)
        {
            return this.Ok();
        }
        
        [HttpPost("application-acceptance")]
        public async Task<IActionResult> AcceptApplicationForm([FromBody]AcceptApplicationRequest request)
        {
            return this.Ok();
        }
        
        [HttpPost("application-rejection")]
        public async Task<IActionResult> RejectApplication([FromBody] RejectApplicationRequest request)
        {
            return this.Ok();
        }
      
        [HttpPost("application-approval")]
        public async Task<IActionResult> ApproveApplication([FromBody] ApproveApplicationRequest request)
        {
            return this.Ok();
        }
        
        [HttpPost("recommendation-request")]
        public async Task<IActionResult> RequestRecommendation([FromBody] ApplicationRecommendationRequest request)
        {
            return this.Ok();
        }
        
        [HttpPost("application-cancellation")]
        public async Task<IActionResult> CancelApplication([FromBody] ApplicationCancellationRequest request)
        {
            return this.Ok();
        }
        
        [HttpPost("application-dismissal")]
        public async Task<IActionResult> DismissApplication([FromBody] DismissApplicationRequest request)
        {
            return this.Ok();
        }
        
        [HttpPost("application-rejection-appeal")]
        public async Task<IActionResult> AppealApplicationRejection([FromBody] ApplicationRejectionAppealRequest request)
        {
            return this.Ok();
        }
        
        [HttpPost("application-endorsement")]
        public async Task<IActionResult> EndorseApplication([FromBody] ApplicationEndorsedRequest request)
        {
            return this.Ok();
        }
        
        [HttpPost("application-oppose")]
        public async Task<IActionResult> OpposeApplication([FromBody] ApplicationOpposedRequest request)
        {
            return this.Ok();
        }
        
        [HttpPost("application-rejection-appeal-dismissal")]
        public async Task<IActionResult> DismissApplicationRejectionAppeal()
        {
            return this.Ok();
        }
        
        [HttpPost("application-rejection-appeal-acceptance")]
        public async Task<IActionResult> AcceptApplicationRejectionAppeal()
        {
            return this.Ok();
        }
        
        [HttpPost("application-rejection-appeal-approval")]
        public async Task<IActionResult> ApproveApplicationRejectionAppeal()
        {
            return this.Ok();
        }
        
        [HttpPost("application-rejection-appeal-rejection")]
        public async Task<IActionResult> RejectApplicationRejectionAppeal()
        {
            return this.Ok();
        }
        
        [HttpPost("application-fee-registration")]
        public async Task<IActionResult> RegisterApplicationFee()
        {
            return this.Ok();
        }
        
    }
}
