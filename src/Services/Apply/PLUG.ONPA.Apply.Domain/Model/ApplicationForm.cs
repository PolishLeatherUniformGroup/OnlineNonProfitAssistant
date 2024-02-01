using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Domain.Model;

public partial class ApplicationForm : AggregateRoot
{
    public NonEmptyString FirstName { get; private set; }
    public NonEmptyString LastName { get; private set; }
    public Address Address { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public NonEmptyString Email { get; private set; }
    public NonEmptyString PhoneNumber { get; private set; }
    public ApplicationStatus Status { get; private set; }
    public Money RequiredMembershipFee { get; private set; }
    public Money? PaidMembershipFee { get; private set; }

    public DateTime ApplicationDate { get; private set; }
    public DateTime? PaymentDate { get; private set; }
    public DateTime? FirstDecisionDate { get; private set; }
    public DateTime? AppealDeadline { get; private set; }
    public DateTime? AppealDate { get; private set; }
    public DateTime? FinalDecisionDate { get; private set; }

    public string? RejectionReason { get; private set; }
    public string? AppealReason { get; private set; }

    private readonly List<ApplicationRecommendation> recommendations = new List<ApplicationRecommendation>();
    public IReadOnlyList<ApplicationRecommendation> Recommendations => this.recommendations;


    public void AcceptApplication(Money requiredFee)
    {
        this.RequiredMembershipFee = requiredFee;
        this.Status = ApplicationStatus.Valid;
    }

    public void ApproveApplication(DateTime decisionDate)
    {
        if (this.Status != ApplicationStatus.AwaitsDecision)
        {
            throw new InvalidOperationException("Cannot approve application that is not awaiting decision");
        }

        if (this.PaidMembershipFee == null || (this.PaidMembershipFee.IsZero && !this.RequiredMembershipFee.IsZero))
        {
            throw new InvalidOperationException("Cannot approve application without paid membership fee");
        }

        if (this.PaidMembershipFee < this.RequiredMembershipFee)
        {
            throw new InvalidOperationException(
                "Cannot approve application with paid membership fee lower than required");
        }

        this.FirstDecisionDate = decisionDate;
        this.Status = ApplicationStatus.Approved;
    }

    public void RejectApplication(DateTime decisionDate, string reason, DateTime appealDeadline)
    {
        this.FirstDecisionDate = decisionDate;
        this.RejectionReason = reason;
        this.AppealDeadline = appealDeadline;
        this.Status = ApplicationStatus.Rejected;
    }


    public void CancelApplication()
    {
    }

    public void RequestRecommendation(DateTime requestDate)
    {
        foreach (var recommendation in this.recommendations)
        {
            recommendation.RequestRecommendation(requestDate);
        }

        this.Status = ApplicationStatus.InRecommendation;
    }

    public void EndorseApplication(Guid recommendationId)
    {
        var recommendation = this.recommendations.Single(r => r.Id == recommendationId);
        recommendation.EndorseRecommendation();
        if (this.recommendations.All(x => x.IsEndorsed))
        {
            this.Status = ApplicationStatus.AwaitsDecision;
        }
    }

    public void OpposeApplication(Guid recommendationId)
    {
        var recommendation = this.recommendations.Single(r => r.Id == recommendationId);
        recommendation.OpposeRecommendation();
        if (this.recommendations.Any(x => x.IsRefused))
        {
            this.Status = ApplicationStatus.Rejected;
        }
    }

    public void AppealRejection(DateTime appealDate, string appealReason)
    {
        if (appealDate > this.AppealDeadline)
        {
            this.FinalDecisionDate = this.AppealDeadline.Value;
            this.Status = ApplicationStatus.AppealDismissed;
            // publish domain event with info about dismissal due to late appeal
        }
        else
        {
            this.AppealDate = appealDate;
            this.AppealReason = appealReason;
            this.Status = ApplicationStatus.RejectionAppealed;
        }
    }

    public void ApproveAppeal(DateTime decisionDate)
    {
        this.FinalDecisionDate = decisionDate;
        this.Status = ApplicationStatus.AppealApproved;
    }

    public void RejectAppeal(DateTime decisionDate)
    {
        this.FinalDecisionDate = decisionDate;
        this.Status = ApplicationStatus.AppealRejected;
    }

    public void RegisterMembershipFeePayment(Money paidFee, DateTime paymentDate)
    {
        this.PaidMembershipFee = paidFee;
        this.PaymentDate = paymentDate;
    }
}