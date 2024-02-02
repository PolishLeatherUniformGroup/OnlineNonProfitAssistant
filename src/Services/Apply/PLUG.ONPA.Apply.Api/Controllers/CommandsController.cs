using System.Net.Mime;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PLUG.ONPA.Apply.Api.Commands;
using PLUG.ONPA.Apply.Api.Requests;
using PLUG.ONPA.Apply.Api.Responses;
using PLUG.ONPA.Common.Application.Dtos;
using PLUG.ONPA.Common.Domain.Exceptions;

namespace PLUG.ONPA.Apply.Api.Controllers
{
    [Route("api/apply/commands")]
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandProcessedResponse))]
    [ProducesErrorResponseType(typeof(ProblemDetails))]
    public class CommandsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public CommandsController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpPost("send-application")]
        public async Task<IActionResult> CreateApplicationForm([FromBody]SendApplicationRequest request)
        {
            var command = this.mapper.Map<CreateApplicationCommand>(request);
            var result = await this.mediator.Send(command);
            return result.Match<IActionResult>(
                success=>
                {
                    var commandResponse = new CommandProcessedResponse(success);
                    commandResponse.AddLink(new GetLink($"/api/apply/application/{success}", "Get Application"));
                    return this.Ok(commandResponse);
                },
                error =>
                {
                    if (error is DomainException)
                    {
                        return this.BadRequest(error.Message);
                    }

                    return this.StatusCode(500, error.Message);
                }); 
        }
        
        [HttpPost("application-acceptance")]
        public async Task<IActionResult> AcceptApplicationForm([FromBody]AcceptApplicationRequest request)
        {
            var command = this.mapper.Map<AcceptApplicationCommand>(request);
            var result = await this.mediator.Send(command);
            return result.Match<IActionResult>(
                success=>
                {
                    var commandResponse = new CommandProcessedResponse(success);
                    commandResponse.AddLink(new GetLink($"/api/apply/application/{success}", "Get Application"));
                    return this.Ok(commandResponse);
                },
                error =>
                {
                    if (error is DomainException)
                    {
                        return this.BadRequest(error.Message);
                    }

                    return this.StatusCode(500, error.Message);
                }); 
        }
        
        [HttpPost("application-rejection")]
        public async Task<IActionResult> RejectApplication([FromBody] RejectApplicationRequest request)
        {
            var command = this.mapper.Map<RejectApplicationCommand>(request);
            var result = await this.mediator.Send(command);
            return result.Match<IActionResult>(
                success=>
                {
                    var commandResponse = new CommandProcessedResponse(success);
                    commandResponse.AddLink(new GetLink($"/api/apply/application/{success}", "Get Application"));
                    return this.Ok(commandResponse);
                },
                error =>
                {
                    if (error is DomainException)
                    {
                        return this.BadRequest(error.Message);
                    }

                    return this.StatusCode(500, error.Message);
                }); 
        }
      
        [HttpPost("application-approval")]
        public async Task<IActionResult> ApproveApplication([FromBody] ApproveApplicationRequest request)
        {
            var command = this.mapper.Map<ApproveApplicationCommand>(request);
            var result = await this.mediator.Send(command);
            return result.Match<IActionResult>(
                success=>
                {
                    var commandResponse = new CommandProcessedResponse(success);
                    commandResponse.AddLink(new GetLink($"/api/apply/application/{success}", "Get Application"));
                    return this.Ok(commandResponse);
                },
                error =>
                {
                    if (error is DomainException)
                    {
                        return this.BadRequest(error.Message);
                    }

                    return this.StatusCode(500, error.Message);
                }); 
        }
        
        [HttpPost("recommendation-request")]
        public async Task<IActionResult> RequestRecommendation([FromBody] ApplicationRecommendationRequest request)
        {
            var command = this.mapper.Map<RequestRecommendationCommand>(request);
            var result = await this.mediator.Send(command);
            return result.Match<IActionResult>(
                success=>
                {
                    var commandResponse = new CommandProcessedResponse(success);
                    commandResponse.AddLink(new GetLink($"/api/apply/application/{success}", "Get Application"));
                    return this.Ok(commandResponse);
                },
                error =>
                {
                    if (error is DomainException)
                    {
                        return this.BadRequest(error.Message);
                    }

                    return this.StatusCode(500, error.Message);
                }); 
        }
        
        [HttpPost("application-cancellation")]
        public async Task<IActionResult> CancelApplication([FromBody] ApplicationCancellationRequest request)
        {
            var command = this.mapper.Map<CancelApplicationCommand>(request);
            var result = await this.mediator.Send(command);
            return result.Match<IActionResult>(
                success=>
                {
                    var commandResponse = new CommandProcessedResponse(success);
                    commandResponse.AddLink(new GetLink($"/api/apply/application/{success}", "Get Application"));
                    return this.Ok(commandResponse);
                },
                error =>
                {
                    if (error is DomainException)
                    {
                        return this.BadRequest(error.Message);
                    }

                    return this.StatusCode(500, error.Message);
                }); 
        }
        
        [HttpPost("application-dismissal")]
        public async Task<IActionResult> DismissApplication([FromBody] DismissApplicationRequest request)
        {
            var command = this.mapper.Map<DismissApplicationCommand>(request);
            var result = await this.mediator.Send(command);
            return result.Match<IActionResult>(
                success=>
                {
                    var commandResponse = new CommandProcessedResponse(success);
                    commandResponse.AddLink(new GetLink($"/api/apply/application/{success}", "Get Application"));
                    return this.Ok(commandResponse);
                },
                error =>
                {
                    if (error is DomainException)
                    {
                        return this.BadRequest(error.Message);
                    }

                    return this.StatusCode(500, error.Message);
                }); 
        }
        
        [HttpPost("application-rejection-appeal")]
        public async Task<IActionResult> AppealApplicationRejection([FromBody] ApplicationRejectionAppealRequest request)
        {
            var command = this.mapper.Map<AppealApplicationRejectionCommand>(request);
            var result = await this.mediator.Send(command);
            return result.Match<IActionResult>(
                success=>
                {
                    var commandResponse = new CommandProcessedResponse(success);
                    commandResponse.AddLink(new GetLink($"/api/apply/application/{success}", "Get Application"));
                    return this.Ok(commandResponse);
                },
                error =>
                {
                    if (error is DomainException)
                    {
                        return this.BadRequest(error.Message);
                    }

                    return this.StatusCode(500, error.Message);
                }); 
        }
        
        [HttpPost("application-endorsement")]
        public async Task<IActionResult> EndorseApplication([FromBody] ApplicationEndorsedRequest request)
        {
            var command = this.mapper.Map<EndorseApplicationCommand>(request);
            var result = await this.mediator.Send(command);
            return result.Match<IActionResult>(
                success=>
                {
                    var commandResponse = new CommandProcessedResponse(success);
                    commandResponse.AddLink(new GetLink($"/api/apply/application/{success}", "Get Application"));
                    return this.Ok(commandResponse);
                },
                error =>
                {
                    if (error is DomainException)
                    {
                        return this.BadRequest(error.Message);
                    }

                    return this.StatusCode(500, error.Message);
                }); 
        }
        
        [HttpPost("application-oppose")]
        public async Task<IActionResult> OpposeApplication([FromBody] ApplicationOpposedRequest request)
        {
            var command = this.mapper.Map<OpposeApplicationCommand>(request);
            var result = await this.mediator.Send(command);
            return result.Match<IActionResult>(
                success=>
                {
                    var commandResponse = new CommandProcessedResponse(success);
                    commandResponse.AddLink(new GetLink($"/api/apply/application/{success}", "Get Application"));
                    return this.Ok(commandResponse);
                },
                error =>
                {
                    if (error is DomainException)
                    {
                        return this.BadRequest(error.Message);
                    }

                    return this.StatusCode(500, error.Message);
                }); 
        }
        
        [HttpPost("application-rejection-appeal-dismissal")]
        public async Task<IActionResult> DismissApplicationRejectionAppeal([FromBody] ApplicationRejectionAppealDismissRequest request)
        {
            var command = this.mapper.Map<DismissApplicationRejectionAppealCommand>(request);
            var result = await this.mediator.Send(command);
            return result.Match<IActionResult>(
                success=>
                {
                    var commandResponse = new CommandProcessedResponse(success);
                    commandResponse.AddLink(new GetLink($"/api/apply/application/{success}", "Get Application"));
                    return this.Ok(commandResponse);
                },
                error =>
                {
                    if (error is DomainException)
                    {
                        return this.BadRequest(error.Message);
                    }

                    return this.StatusCode(500, error.Message);
                }); 
        }
        
        
        [HttpPost("application-rejection-appeal-acceptance")]
        public async Task<IActionResult> AcceptApplicationRejectionAppeal([FromBody] ApplicationRejectionAppealAcceptRequest request)
        {
            {
                var command = this.mapper.Map<AcceptApplicationRejectionAppealCommand>(request);
                var result = await this.mediator.Send(command);
                return result.Match<IActionResult>(
                    success=>
                    {
                        var commandResponse = new CommandProcessedResponse(success);
                        commandResponse.AddLink(new GetLink($"/api/apply/application/{success}", "Get Application"));
                        return this.Ok(commandResponse);
                    },
                    error =>
                    {
                        if (error is DomainException)
                        {
                            return this.BadRequest(error.Message);
                        }

                        return this.StatusCode(500, error.Message);
                    }); 
            }
        }
        
        [HttpPost("application-rejection-appeal-approval")]
        public async Task<IActionResult> ApproveApplicationRejectionAppeal([FromBody] ApplicationRejectionAppealApproveRequest request)
        {
            {
                var command = this.mapper.Map<ApproveApplicationRejectionAppealCommand>(request);
                var result = await this.mediator.Send(command);
                return result.Match<IActionResult>(
                    success=>
                    {
                        var commandResponse = new CommandProcessedResponse(success);
                        commandResponse.AddLink(new GetLink($"/api/apply/application/{success}", "Get Application"));
                        return this.Ok(commandResponse);
                    },
                    error =>
                    {
                        if (error is DomainException)
                        {
                            return this.BadRequest(error.Message);
                        }

                        return this.StatusCode(500, error.Message);
                    }); 
            }
        }
        
        [HttpPost("application-rejection-appeal-rejection")]
        public async Task<IActionResult> RejectApplicationRejectionAppeal([FromBody] ApplicationRejectionAppealRejectRequest request)
        {
            {
                var command = this.mapper.Map<RejectApplicationRejectionAppealCommand>(request);
                var result = await this.mediator.Send(command);
                return result.Match<IActionResult>(
                    success=>
                    {
                        var commandResponse = new CommandProcessedResponse(success);
                        commandResponse.AddLink(new GetLink($"/api/apply/application/{success}", "Get Application"));
                        return this.Ok(commandResponse);
                    },
                    error =>
                    {
                        if (error is DomainException)
                        {
                            return this.BadRequest(error.Message);
                        }

                        return this.StatusCode(500, error.Message);
                    }); 
            }
        }
        
        [HttpPost("application-fee-registration")]
        public async Task<IActionResult> RegisterApplicationFee([FromBody] ApplicationFeeRegistrationRequest request)
        {
            {
                var command = this.mapper.Map<RegisterApplicationFeePaymentCommand>(request);
                var result = await this.mediator.Send(command);
                return result.Match<IActionResult>(
                    success=>
                    {
                        var commandResponse = new CommandProcessedResponse(success);
                        commandResponse.AddLink(new GetLink($"/api/apply/application/{success}", "Get Application"));
                        return this.Ok(commandResponse);
                    },
                    error =>
                    {
                        if (error is DomainException)
                        {
                            return this.BadRequest(error.Message);
                        }

                        return this.StatusCode(500, error.Message);
                    }); 
            }
        }
        
    }
}
