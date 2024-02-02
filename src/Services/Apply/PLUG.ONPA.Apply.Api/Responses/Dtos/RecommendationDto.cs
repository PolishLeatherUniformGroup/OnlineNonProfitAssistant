namespace PLUG.ONPA.Apply.Api.Responses.Dtos;

public sealed class RecommendationDto
{
    public string Id { get; set; }
    public string CardNumber { get; set; }
    public bool IsValid { get; set; }
    public bool? Status { get; set; }
    public DateTime? RequestedAt { get; set; }
}