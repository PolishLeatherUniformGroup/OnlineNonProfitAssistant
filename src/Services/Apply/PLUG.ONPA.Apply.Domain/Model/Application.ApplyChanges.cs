using PLUG.ONPA.Apply.Domain.ChangeEvents;

namespace PLUG.ONPA.Apply.Domain.Model;

public partial class ApplicationAggregate
{
    public void ApplyChange(ApplicationCreatedChangeEvent @event)
    {
        this.FirstName = @event.FirstName;
        this.LastName = @event.LastName;
        this.Address = @event.Address;
        this.BirthDate = @event.BirthDate;
        this.Email = @event.Email;
        this.PhoneNumber = @event.PhoneNumber;
        this.Status = @event.Status;
        this.ApplicationDate = @event.ApplicationDate;
        foreach (var recommendation in @event.Recommendations)
        {
            this.recommendations.Add(recommendation);
        }
    }
    
    public void ApplyChange(ApplicationAcceptedChangeEvent @event)
    {
        this.RequiredMembershipFee = @event.RequiredFee;
        this.Status = @event.Status;
        foreach (var recommendation in this.recommendations)
        {
            recommendation.MarkAsValid();
        }
    }
    
    public void ApplyChange(ApplicationApprovedChangeEvent @event)
    {
        this.Status = @event.Status;
        this.FirstDecisionDate = @event.DecisionDate;
    }
    
    public void ApplyChange(ApplicationDismissedChangeEvent @event)
    {
        this.Status = @event.Status;
        foreach (var recommendation in this.recommendations)
        {
            if(@event.ValidRecommendations.Contains(recommendation.Recommender))
            {
                recommendation.MarkAsValid();
            }
        }
    }
    
    public void ApplyChange(ApplicationRejectedChangeEvent @event)
    {
        this.Status = @event.Status;
        this.RejectionReason = @event.DecisionReason;
        this.FirstDecisionDate = @event.DecisionDate;
        this.AppealDeadline = @event.AppealDeadline;
    }
    
    public void ApplyChange(ApplicationCancelledChangeEvent @event)
    {
        this.Status = @event.Status;
        this.CancellationDate = @event.CancellationDate;
    }
    
    public void ApplyChange(ApplicationRecommendationRequestedChangeEvent @event)
    {
        this.Status = @event.Status;
        foreach (var recommendation in this.recommendations)
        {
            recommendation.RequestRecommendation(@event.RequestedAt);   
        }
    }
    
    public void ApplyChange(ApplicationEndorsedChangeEvent @event)
    {
        var recommendation = this.recommendations.Single(r => r.Id == @event.RecommendationId);
        recommendation.EndorseRecommendation();
    }
    
    public void ApplyChange(ApplicationRecommendedChangeEvent @event)
    {
        this.Status = @event.Status;
    }
    
    public void ApplyChange(ApplicationOpposedChangeEvent @event)
    {
        var recommendation = this.recommendations.Single(r => r.Id == @event.RecommendationId);
        recommendation.OpposeRecommendation();
        this.Status = @event.Status;
    }
    
    public void ApplyChange(ApplicationRejectionAppealDismissedChangeEvent @event)
    {
        this.Status = @event.Status;
        this.FinalDecisionDate = @event.AppealDate;
    }
    
    public void ApplyChange(ApplicationRejectionAppealApprovedChangeEvent @event)
    {
        this.Status = @event.Status;
        this.FinalDecisionDate = @event.DecisionDate;
        this.FinalDecisionReason = @event.DecisionReason;
    }
    
    public void ApplyChange(ApplicationRejectionAppealRejectedChangeEvent @event)
    {
        this.Status = @event.Status;
        this.FinalDecisionDate = @event.DecisionDate;
        this.FinalDecisionReason = @event.DecisionReason;
    }
    
    public void ApplyChange(ApplicationMembershipFeePaymentReceivedChangeEvent @event)
    {
        this.PaymentDate = @event.PaidDate;
        this.PaidMembershipFee = @event.PaidFee;
    }
}