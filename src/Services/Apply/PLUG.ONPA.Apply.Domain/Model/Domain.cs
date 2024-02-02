using PLUG.ONPA.Apply.Domain.ChangeEvents;
using PLUG.ONPA.Apply.Domain.DomainEvents;
using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Exceptions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Domain.Model;

public partial class Domain : AggregateRoot
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
    public DateTime? CancellationDate { get; private set; }
    public string? RejectionReason { get; private set; }
    public string? AppealReason { get; private set; }

    public string? FinalDecisionReason { get; private set; }

    private readonly List<ApplicationRecommendation> recommendations = new List<ApplicationRecommendation>();
    public IReadOnlyList<ApplicationRecommendation> Recommendations => this.recommendations;

    public Domain(NonEmptyString firstName, NonEmptyString lastName, Address address, DateOnly birthDate,
        NonEmptyString email, NonEmptyString phoneNumber, List<ApplicationRecommendation> recommendations,
        DateTime applicationDate,
        Guid? tenantId = null)
        : base(tenantId)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Address = address;
        this.BirthDate = birthDate;
        this.Email = email;
        this.PhoneNumber = phoneNumber;
        this.recommendations = recommendations;
        this.ApplicationDate = applicationDate;
        this.Status = ApplicationStatus.Received;

        var domainEvent = new ApplicationReceivedDomainEvent(firstName, lastName, address, birthDate, email,
            phoneNumber, recommendations, applicationDate);
        this.RaiseDomainEvent(domainEvent);

        var changeEvent = new ApplicationCreatedChangeEvent(firstName, lastName, address, birthDate, email,
            phoneNumber, ApplicationStatus.Received, applicationDate, recommendations);
        this.EmmitChangeEvent(changeEvent);
    }

    public void AcceptApplication(Money requiredFee)
    {
        this.RequiredMembershipFee = requiredFee;
        foreach (var recommendation in this.recommendations)
        {
            recommendation.MarkAsValid();
        }

        this.Status = ApplicationStatus.Valid;

        var domainEvent = new ApplicationAcceptedDomainEvent(requiredFee, this.Email);
        this.RaiseDomainEvent(domainEvent);

        var changeEvent = new ApplicationAcceptedChangeEvent(this.Status, this.RequiredMembershipFee);
        this.EmmitChangeEvent(changeEvent);
    }

    public void DismissApplication(List<CardNumber> validRecommenders)
    {
        foreach (var recommendation in this.recommendations)
        {
            if (validRecommenders.Contains(recommendation.Recommender))
            {
                recommendation.MarkAsValid();
            }
        }

        this.Status = ApplicationStatus.Invalid;

        var domainEvent = new ApplicationDismissedDomainEvent(this.Email);
        this.RaiseDomainEvent(domainEvent);

        var changeEvent = new ApplicationDismissedChangeEvent(this.Status, validRecommenders);
        this.EmmitChangeEvent(changeEvent);
    }

    public void ApproveApplication(DateTime decisionDate)
    {
        if (this.Status != ApplicationStatus.AwaitsDecision)
        {
            throw new DomainException("Cannot approve application that is not awaiting decision");
        }

        if (this.PaidMembershipFee == null || (this.PaidMembershipFee.IsZero && !this.RequiredMembershipFee.IsZero))
        {
            throw new DomainException("Cannot approve application without paid membership fee");
        }

        if (this.PaidMembershipFee < this.RequiredMembershipFee)
        {
            throw new DomainException(
                "Cannot approve application with paid membership fee lower than required");
        }

        this.FirstDecisionDate = decisionDate;
        this.Status = ApplicationStatus.Approved;

        var domainEvent = new ApplicationApprovedDomainEvent(decisionDate, this.Email);
        this.RaiseDomainEvent(domainEvent);

        var changeEvent = new ApplicationApprovedChangeEvent(this.FirstDecisionDate.Value, this.Status);
        this.EmmitChangeEvent(changeEvent);
    }

    public void RejectApplication(DateTime decisionDate, string reason, DateTime appealDeadline)
    {
        if (this.Status != ApplicationStatus.AwaitsDecision)
        {
            throw new DomainException("Cannot approve application that is not awaiting decision");
        }

        this.FirstDecisionDate = decisionDate;
        this.RejectionReason = reason;
        this.AppealDeadline = appealDeadline;
        this.Status = ApplicationStatus.Rejected;

        var domainEvent = new ApplicationRejectedDomainEvent(decisionDate, this.Email, reason, appealDeadline);
        this.RaiseDomainEvent(domainEvent);

        var changeEvent = new ApplicationRejectedChangeEvent(this.Status, this.FirstDecisionDate.Value,
            this.RejectionReason, this.AppealDeadline.Value);
        this.EmmitChangeEvent(changeEvent);
    }

    public void CancelApplication(DateTime dateTime)
    {
        if (this.Status == ApplicationStatus.Cancelled)
        {
            throw new DomainException("Cannot cancel application that is already cancelled");
        }

        if (this.Status == ApplicationStatus.Approved)
        {
            throw new DomainException("Cannot cancel application that is already approved");
        }

        if (this.Status == ApplicationStatus.AppealApproved)
        {
            throw new DomainException("Cannot cancel application that is already appeal approved");
        }

        if (this.Status == ApplicationStatus.AppealRejected)
        {
            throw new DomainException("Cannot cancel application that is already appeal rejected");
        }

        this.Status = ApplicationStatus.Cancelled;
        this.CancellationDate = dateTime;

        var domainEvent = new ApplicationCancelledDomainEvent(this.Email, dateTime);
        this.RaiseDomainEvent(domainEvent);

        var changeEvent = new ApplicationCancelledChangeEvent(this.CancellationDate.Value, this.Status);
        this.EmmitChangeEvent(changeEvent);
    }

    public void RequestRecommendation(DateTime requestDate)
    {
        foreach (var recommendation in this.recommendations)
        {
            recommendation.RequestRecommendation(requestDate);
            var domainEvent =
                new ApplicationRecommendationRequestedDomainEvent(recommendation.Recommender, requestDate);
            this.RaiseDomainEvent(domainEvent);
        }

        this.Status = ApplicationStatus.InRecommendation;

        var changeEvent = new ApplicationRecommendationRequestedChangeEvent(requestDate, this.Status);
        this.EmmitChangeEvent(changeEvent);
    }

    public void EndorseApplication(Guid recommendationId)
    {
        var recommendation = this.recommendations.SingleOrDefault(r => r.Id == recommendationId);
        if (recommendation == null)
        {
            throw new DomainException("Cannot endorse application with recommendation that does not exist");
        }

        recommendation.EndorseRecommendation();

        var changeEvent = new ApplicationEndorsedChangeEvent(recommendationId);
        this.EmmitChangeEvent(changeEvent);

        if (this.recommendations.All(x => x.IsEndorsed))
        {
            this.Status = ApplicationStatus.AwaitsDecision;
            var domainEvent = new ApplicationEndorsedDomainEvent(this.Email);
            this.RaiseDomainEvent(domainEvent);

            var changeEvent2 = new ApplicationRecommendedChangeEvent(this.Status);
            this.EmmitChangeEvent(changeEvent2);
        }
    }

    public void OpposeApplication(Guid recommendationId)
    {
        var recommendation = this.recommendations.SingleOrDefault(r => r.Id == recommendationId);
        if (recommendation == null)
        {
            throw new DomainException("Cannot oppose application with recommendation that does not exist");
        }

        recommendation.OpposeRecommendation();
        if (this.recommendations.Any(x => x.IsRefused))
        {
            this.Status = ApplicationStatus.Rejected;
            var domainEvent = new ApplicationOpposedDomainEvent(this.Email);
            this.RaiseDomainEvent(domainEvent);
        }

        var changeEvent = new ApplicationOpposedChangeEvent(this.Status, recommendationId);
        this.EmmitChangeEvent(changeEvent);
    }

    public void AppealRejection(DateTime appealDate, string appealReason)
    {
        if (this.Status != ApplicationStatus.Rejected)
        {
            throw new DomainException("Cannot appeal rejection of application that is not rejected");
        }

        if (appealDate > this.AppealDeadline)
        {
            this.FinalDecisionDate = this.AppealDeadline.Value;
            this.Status = ApplicationStatus.AppealDismissed;

            var domainEvent =
                new ApplicationRejectionAppealDismissedDomainEvent(this.Email, this.AppealDeadline.Value, appealDate);
            this.RaiseDomainEvent(domainEvent);

            var changeEvent = new ApplicationRejectionAppealDismissedChangeEvent(appealDate, this.Status);
            this.EmmitChangeEvent(changeEvent);
        }
        else
        {
            this.AppealDate = appealDate;
            this.AppealReason = appealReason;
            this.Status = ApplicationStatus.RejectionAppealed;

            var domainEvent = new ApplicationRejectionAppealReceivedDomainEvent(this.Email, appealDate, appealReason);
            this.RaiseDomainEvent(domainEvent);

            var changeEvent =
                new ApplicationRejectionAppealReceivedChangeEvent(appealDate, this.AppealReason, this.Status);
            this.EmmitChangeEvent(changeEvent);
        }
    }

    public void ApproveAppeal(DateTime decisionDate, string reason)
    {
        if (this.Status != ApplicationStatus.RejectionAppealed)
        {
            throw new DomainException("Cannot approve appeal of application that is not appealed");
        }

        this.FinalDecisionDate = decisionDate;
        this.FinalDecisionReason = reason;
        this.Status = ApplicationStatus.AppealApproved;

        var domainEvent = new ApplicationRejectionAppealApprovedDomainEvent(this.Email, decisionDate, reason);
        this.RaiseDomainEvent(domainEvent);

        var changeEvent =
            new ApplicationRejectionAppealApprovedChangeEvent(decisionDate, this.FinalDecisionReason, this.Status);
        this.EmmitChangeEvent(changeEvent);
    }

    public void RejectAppeal(DateTime decisionDate, string reason)
    {
        if (this.Status != ApplicationStatus.RejectionAppealed)
        {
            throw new DomainException("Cannot reject appeal of application that is not appealed");
        }

        this.FinalDecisionDate = decisionDate;
        this.FinalDecisionReason = reason;
        this.Status = ApplicationStatus.AppealRejected;

        var domainEvent = new ApplicationRejectionAppealRejectedDomainEvent(this.Email, decisionDate, reason);
        this.RaiseDomainEvent(domainEvent);

        var changeEvent =
            new ApplicationRejectionAppealRejectedChangeEvent(decisionDate, this.FinalDecisionReason, this.Status);
        this.EmmitChangeEvent(changeEvent);
    }

    public void RegisterMembershipFeePayment(Money paidFee, DateTime paymentDate)
    {
        if (this.Status != ApplicationStatus.Valid && this.Status != ApplicationStatus.AwaitsDecision &&
            this.Status != ApplicationStatus.InRecommendation)
        {
            throw new DomainException("Cannot register membership fee payment for application that is not valid");
        }

        this.PaidMembershipFee = paidFee;
        this.PaymentDate = paymentDate;

        var domainEvent = new ApplicationMembershipFeePaidDomainEvent(this.Email, paymentDate, paidFee);
        this.RaiseDomainEvent(domainEvent);

        var changeEvent = new ApplicationMembershipFeePaymentReceivedChangeEvent(paymentDate, paidFee);
        this.EmmitChangeEvent(changeEvent);
    }
}