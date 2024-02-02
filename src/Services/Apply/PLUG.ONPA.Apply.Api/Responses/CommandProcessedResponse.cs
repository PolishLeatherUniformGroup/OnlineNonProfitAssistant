using PLUG.ONPA.Common.Application.Dtos;

namespace PLUG.ONPA.Apply.Api.Responses;

public sealed class CommandProcessedResponse : CommandResponse
{
    public string Id { get; set; }

    public CommandProcessedResponse(Guid aggregateId)
    {
        this.Id = aggregateId.ToString("N");
    }
}