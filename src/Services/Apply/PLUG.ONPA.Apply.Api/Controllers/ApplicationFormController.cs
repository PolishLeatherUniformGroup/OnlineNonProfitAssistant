using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PLUG.ONPA.Apply.Api.Controllers
{
    [Route("api/application-forms")]
    [ApiController]
    public class ApplicationFormController : ControllerBase
    {
        private readonly IMediator mediator;

        public ApplicationFormController(IMediator mediator)
        {
            this.mediator = mediator;
        }
    }
}
