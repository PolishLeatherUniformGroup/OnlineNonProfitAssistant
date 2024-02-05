namespace PLUG.ONPA.Apply.Read.Models;

public sealed class Recommendation
{
    public string Id { get; set; }
    public string Recommender { get; set; }
    public bool IsValid { get; set; }
    public DateTime? RequestedAt { get; set; }
    public bool? Status { get; set; }
}