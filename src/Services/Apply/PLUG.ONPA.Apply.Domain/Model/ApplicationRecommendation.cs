using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Domain.Model;

public class ApplicationRecommendation :Entity
{
    public CardNumber Recommender { get; private set; }
    public bool IsValid { get; private set; }
    public DateTime? RequestedAt { get; private set; }
    private bool? isEndorsed;
    
    public bool IsEndorsed => this.isEndorsed.GetValueOrDefault(false);
    public bool IsRefused => this.isEndorsed.HasValue && !this.IsEndorsed;
    
    public void MarkAsValid()
    {
        this.IsValid = true;
    }
    
    public void RequestRecommendation(DateTime requestedAt)
    {
        if (this.IsValid)
        {
            this.RequestedAt = requestedAt;
        }
        else
        {
            throw new InvalidOperationException("Cannot request recommendation for invalid recommendation");
        }
    }

    public void EndorseRecommendation()
    {
        if(this.IsValid && this.RequestedAt.HasValue)
        {
            this.isEndorsed = true;
        }
        else
        {
            throw new InvalidOperationException("Cannot endorse recommendation that is not valid or has not been requested");
        }
    }
    
    public void OpposeRecommendation()
    {
        if(this.IsValid && this.RequestedAt.HasValue)
        {
            this.isEndorsed = false;
        }
        else
        {
            throw new InvalidOperationException("Cannot oppose recommendation that is not valid or has not been requested");
        }
    }
    
}