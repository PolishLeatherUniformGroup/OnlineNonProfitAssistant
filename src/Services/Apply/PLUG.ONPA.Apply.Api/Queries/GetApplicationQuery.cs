using PLUG.ONPA.Common.Application;
using PLUG.ONPA.Apply.Read.Models;

namespace PLUG.ONPA.Apply.Api.Queries;

public record class GetApplicationQuery(Guid ApplicationId) : QueryBase<Application>
{
       
}