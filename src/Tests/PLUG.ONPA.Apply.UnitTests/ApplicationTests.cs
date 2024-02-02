using AutoFixture;
using FluentAssertions;
using PLUG.ONPA.Apply.Domain.ChangeEvents;
using PLUG.ONPA.Apply.Domain.DomainEvents;
using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Domain.Exceptions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.UnitTests;

public class ApplicationTests
{
    private readonly IFixture fixture;

    public ApplicationTests()
    {
        this.fixture = new Fixture();
    }
    
    [Fact]
    public void CreateApplicationForm_should_emmit_ApplicationFormCreatedChangeEvent()
    {
        // Arrange
        var firstName = this.fixture.Create<string>();
        var lastName = this.fixture.Create<string>();
        var addressCountry = this.fixture.Create<string>();
        var addressCity = this.fixture.Create<string>();
        var addressStreet = this.fixture.Create<string>();
        var addressNumber = this.fixture.Create<string>();
        var address = new Address(addressCountry, addressCity, addressNumber, addressStreet);
        var (birthDate, _) = this.fixture.Create<DateTime>();
        var email = this.fixture.Create<string>();
        var phoneNumber = this.fixture.Create<string>();
        var applicationDate = this.fixture.Create<DateTime>();
        var recommender = this.fixture.Create<string>();
        var recommendation = new ApplicationRecommendation(Guid.NewGuid(),
            new CardNumber(this.fixture.Create<string>(), this.fixture.Create<int>()));
        
        // Act
        var sut = new Application(firstName,
            lastName,
            address,
            birthDate,
            email,
            phoneNumber,
            new List<ApplicationRecommendation>()
            {
                recommendation
            },
            applicationDate);
        
        // Assert
        sut.Should().NotBeNull();
        sut.FirstName.Value.Should().BeEquivalentTo(firstName);
        sut.LastName.Value.Should().BeEquivalentTo(lastName);
        sut.Address.Should().Be(address);
        sut.BirthDate.Should().Be(birthDate);
        sut.Email.Value.Should().BeEquivalentTo(email);
        sut.PhoneNumber.Value.Should().BeEquivalentTo(phoneNumber);
        sut.ApplicationDate.Should().Be(applicationDate);
        sut.Status.Should().Be(ApplicationStatus.Received);
        sut.Recommendations.Should().HaveCount(1);
        sut.Recommendations.Should().Contain(recommendation);
        sut.GetChangeEvents().Should().HaveCount(1);
        sut.GetChangeEvents().Should().ContainItemsAssignableTo<ApplicationCreatedChangeEvent>();
        sut.GetDomainEvents().Should().HaveCount(1);
        sut.GetDomainEvents().Should().ContainItemsAssignableTo<ApplicationReceivedDomainEvent>();
    }
    
    [Fact]
    public void AcceptApplicationForm_should_emmit_ApplicationAcceptedChangeEvent()
    {
        // Arrange
        var sut = this.CreateValidApplicationForm();
        var requiredFee = this.fixture.Create<decimal>();
        
        // Act
        sut.AcceptApplication(Money.FromPln(requiredFee));
        
        // Assert
        sut.Status.Should().Be(ApplicationStatus.Valid);
        sut.RequiredMembershipFee.Amount.Should().Be(requiredFee);
        sut.RequiredMembershipFee.Currency.Value.Should().Be(Money.Pln);
        sut.Recommendations.Should().HaveCount(1);
        sut.Recommendations.All(x=>x.IsValid).Should().BeTrue();
        sut.GetChangeEvents().Should().HaveCount(1);
        sut.GetChangeEvents().Should().ContainItemsAssignableTo<ApplicationAcceptedChangeEvent>();
        sut.GetDomainEvents().Should().HaveCount(1);
        sut.GetDomainEvents().Should().ContainItemsAssignableTo<ApplicationAcceptedDomainEvent>();
        
    }
    
    [Fact]
    public void DismissApplicationForm_should_emmit_ApplicationDismissedChangeEvent()
    {
        // Arrange
        var sut = this.CreateValidApplicationForm();
        
        
        // Act
        sut.DismissApplication(new List<CardNumber>());
        
        // Assert
        sut.Status.Should().Be(ApplicationStatus.Invalid);
        sut.Recommendations.Should().HaveCount(1);
        sut.Recommendations.All(x=>!x.IsValid).Should().BeTrue();
        sut.GetChangeEvents().Should().HaveCount(1);
        sut.GetChangeEvents().Should().ContainItemsAssignableTo<ApplicationDismissedChangeEvent>();
        sut.GetDomainEvents().Should().HaveCount(1);
        sut.GetDomainEvents().Should().ContainItemsAssignableTo<ApplicationDismissedDomainEvent>();
    }
    
    [Fact]
    public void RequestRecommendation_should_emmit_ApplicationRecommendationRequestedChangeEvent()
    {
        // Arrange
        var sut = this.CreateValidApplicationForm();
        var requiredFee = this.fixture.Create<decimal>();
        sut.AcceptApplication(Money.FromPln(requiredFee));
        var requestedAt = this.fixture.Create<DateTime>();
        
        // Act
        sut.RequestRecommendation(requestedAt);
        
        // Assert
        sut.Recommendations.Should().HaveCount(1);
        sut.Recommendations.All(x=>x.RequestedAt == requestedAt).Should().BeTrue();
        sut.Status.Should().Be(ApplicationStatus.InRecommendation);
        sut.GetChangeEvents().Should().HaveCount(2);
        sut.GetChangeEvents().Should().ContainItemsAssignableTo<ApplicationRecommendationRequestedChangeEvent>();
        sut.GetDomainEvents().Should().HaveCount(2);
        sut.GetDomainEvents().Should().ContainItemsAssignableTo<ApplicationRecommendationRequestedDomainEvent>();
    }
    
    [Fact]
    public void EndorseApplication_should_emmit_ApplicationEndorsedChangeEvent()
    {
        // Arrange
        var sut = this.CreateValidApplicationForm();
        var requiredFee = this.fixture.Create<decimal>();
        sut.AcceptApplication(Money.FromPln(requiredFee));
        sut.RequestRecommendation(this.fixture.Create<DateTime>());
        
        // Act
        sut.EndorseApplication(sut.Recommendations.First().Id);
        
        // Assert
        sut.Recommendations.Should().HaveCount(1);
        sut.Recommendations.All(x=>x.IsEndorsed).Should().BeTrue();
        sut.Status.Should().Be(ApplicationStatus.AwaitsDecision);
        sut.GetChangeEvents().Should().HaveCount(4);
        sut.GetChangeEvents().Should().ContainItemsAssignableTo<ApplicationEndorsedChangeEvent>();
        sut.GetChangeEvents().Should().ContainItemsAssignableTo<ApplicationRecommendedChangeEvent>();
        sut.GetDomainEvents().Should().HaveCount(3);
        sut.GetDomainEvents().Should().ContainItemsAssignableTo<ApplicationEndorsedDomainEvent>();
    }
    
    [Fact]
    public void EndorseApplication_should_throwException_when_recommendationNotFound()
    {
        // Arrange
        var sut = this.CreateValidApplicationForm();
        var requiredFee = this.fixture.Create<decimal>();
        sut.AcceptApplication(Money.FromPln(requiredFee));
        sut.RequestRecommendation(this.fixture.Create<DateTime>());
        
        // Act
       var result = ()=> sut.EndorseApplication(Guid.NewGuid());
        
        // Assert
       result.Should().Throw<DomainException>();
       
    }
    
    [Fact]
    public void OpposeApplication_should_emmit_ApplicationEndorsedChangeEvent()
    {
        // Arrange
        var sut = this.CreateValidApplicationForm();
        var requiredFee = this.fixture.Create<decimal>();
        sut.AcceptApplication(Money.FromPln(requiredFee));
        sut.RequestRecommendation(this.fixture.Create<DateTime>());
        
        // Act
        sut.OpposeApplication(sut.Recommendations.First().Id);
        
        // Assert
        sut.Recommendations.Should().HaveCount(1);
        sut.Recommendations.All(x=>x.IsRefused).Should().BeTrue();
        sut.Status.Should().Be(ApplicationStatus.Rejected);
        sut.GetChangeEvents().Should().HaveCount(3);
        sut.GetChangeEvents().Should().ContainItemsAssignableTo<ApplicationOpposedChangeEvent>();
        sut.GetDomainEvents().Should().HaveCount(3);
        sut.GetDomainEvents().Should().ContainItemsAssignableTo<ApplicationOpposedDomainEvent>();
    }
    
    [Fact]
    public void OpposeApplication_should_throwException_when_recommendationNotFound()
    {
        // Arrange
        var sut = this.CreateValidApplicationForm();
        var requiredFee = this.fixture.Create<decimal>();
        sut.AcceptApplication(Money.FromPln(requiredFee));
        sut.RequestRecommendation(this.fixture.Create<DateTime>());
        
        // Act
        var result = ()=> sut.OpposeApplication(Guid.NewGuid());
        
        // Assert
        result.Should().Throw<DomainException>();
       
    }
    
    [Fact]
    public void RegisterMembershipFeePayment_should_emmit_ApplicationMembershipFeePaymentReceivedChangeEvent()
    {
        // Arrange
        var sut = this.CreateValidApplicationForm();
        var requiredFee = this.fixture.Create<decimal>();
        sut.AcceptApplication(Money.FromPln(requiredFee));
        sut.RequestRecommendation(this.fixture.Create<DateTime>());
        var paidDate = this.fixture.Create<DateTime>();
        // Act
        sut.RegisterMembershipFeePayment(sut.RequiredMembershipFee,paidDate);
        
        // Assert
        sut.Recommendations.Should().HaveCount(1);
        sut.Status.Should().Be(ApplicationStatus.InRecommendation);
        sut.PaidMembershipFee.Should().NotBeNull();
        sut.PaidMembershipFee!.Amount.Should().Be(requiredFee);
        sut.PaymentDate.Should().Be(paidDate);
        sut.GetChangeEvents().Should().HaveCount(3);
        sut.GetChangeEvents().Should().ContainItemsAssignableTo<ApplicationMembershipFeePaymentReceivedChangeEvent>();
        sut.GetDomainEvents().Should().HaveCount(3);
        sut.GetDomainEvents().Should().ContainItemsAssignableTo<ApplicationMembershipFeePaidDomainEvent>();
    }

    [Fact]
    public void ApproveApplication_should_emmit_ApplicationApprovedChangeEvent()
    {
        // Arrange
        var sut = this.CreateValidApplicationForm();
        var requiredFee = this.fixture.Create<decimal>();
        sut.AcceptApplication(Money.FromPln(requiredFee));
        sut.RequestRecommendation(this.fixture.Create<DateTime>());
        sut.EndorseApplication(sut.Recommendations.First().Id);
        var approveDate = this.fixture.Create<DateTime>();
        sut.RegisterMembershipFeePayment(sut.RequiredMembershipFee,approveDate);
        // Act
        sut.ApproveApplication(approveDate);
        
        // Assert
        sut.Recommendations.Should().HaveCount(1);
        sut.Recommendations.All(x=>x.IsEndorsed).Should().BeTrue();
        sut.Status.Should().Be(ApplicationStatus.Approved);
        sut.GetChangeEvents().Should().HaveCount(6);
        sut.GetChangeEvents().Should().ContainItemsAssignableTo<ApplicationApprovedChangeEvent>();
        sut.GetDomainEvents().Should().HaveCount(5);
        sut.GetDomainEvents().Should().ContainItemsAssignableTo<ApplicationApprovedDomainEvent>();
    }
    
    [Fact]
    public void RejectApplication_should_emmit_ApplicationRejectedChangeEvent()
    {
        // Arrange
        var sut = this.CreateValidApplicationForm();
        var requiredFee = this.fixture.Create<decimal>();
        sut.AcceptApplication(Money.FromPln(requiredFee));
        sut.RequestRecommendation(this.fixture.Create<DateTime>());
        sut.EndorseApplication(sut.Recommendations.First().Id);
        var rejectionDate = this.fixture.Create<DateTime>();
        sut.RegisterMembershipFeePayment(sut.RequiredMembershipFee,rejectionDate);
        var reason = this.fixture.Create<string>();
        var deadline = this.fixture.Create<DateTime>();
        // Act
        sut.RejectApplication(rejectionDate,reason,deadline);
        
        // Assert
        sut.Recommendations.Should().HaveCount(1);
        sut.Recommendations.All(x=>x.IsEndorsed).Should().BeTrue();
        sut.RejectionReason.Should().Be(reason);
        sut.AppealDeadline.Should().Be(deadline);
        sut.FirstDecisionDate.Should().Be(rejectionDate);
        sut.Status.Should().Be(ApplicationStatus.Rejected);
        sut.GetChangeEvents().Should().HaveCount(6);
        sut.GetChangeEvents().Should().ContainItemsAssignableTo<ApplicationRejectedChangeEvent>();
        sut.GetDomainEvents().Should().HaveCount(5);
        sut.GetDomainEvents().Should().ContainItemsAssignableTo<ApplicationRejectedDomainEvent>();
    }
    
    [Fact]
    public void AppealApplicationRejection_should_emmit_ApplicationRejectionAppealDismissedChangeEvent_whenAppealedOverDeadline()
    {
        // Arrange
        var sut = this.CreateValidApplicationForm();
        var requiredFee = this.fixture.Create<decimal>();
        sut.AcceptApplication(Money.FromPln(requiredFee));
        sut.RequestRecommendation(this.fixture.Create<DateTime>());
        sut.EndorseApplication(sut.Recommendations.First().Id);
        var rejectionDate = this.fixture.Create<DateTime>();
        sut.RegisterMembershipFeePayment(sut.RequiredMembershipFee,rejectionDate);
        var reason = this.fixture.Create<string>();
        var deadline = this.fixture.Create<DateTime>();
        sut.RejectApplication(rejectionDate,reason,deadline);
        
        // Act
        sut.AppealRejection(deadline.AddDays(2),this.fixture.Create<string>());
        
        // Assert
        sut.Recommendations.Should().HaveCount(1);
        sut.Recommendations.All(x=>x.IsEndorsed).Should().BeTrue();
        sut.RejectionReason.Should().Be(reason);
        sut.AppealDeadline.Should().Be(deadline);
        sut.FinalDecisionDate.Should().Be(deadline);
        sut.Status.Should().Be(ApplicationStatus.AppealDismissed);
        sut.GetChangeEvents().Should().HaveCount(7);
        sut.GetChangeEvents().Should().ContainItemsAssignableTo<ApplicationRejectionAppealDismissedChangeEvent>();
        sut.GetDomainEvents().Should().HaveCount(6);
        sut.GetDomainEvents().Should().ContainItemsAssignableTo<ApplicationRejectionAppealDismissedDomainEvent>();
    }
    
    [Fact]
    public void AppealApplicationRejection_should_emmit_ApplicationRejectionAppealReceivedChangeEvent_whenAppealedBeforeDeadline()
    {
        // Arrange
        var sut = this.CreateValidApplicationForm();
        var requiredFee = this.fixture.Create<decimal>();
        sut.AcceptApplication(Money.FromPln(requiredFee));
        sut.RequestRecommendation(this.fixture.Create<DateTime>());
        sut.EndorseApplication(sut.Recommendations.First().Id);
        var rejectionDate = this.fixture.Create<DateTime>();
        sut.RegisterMembershipFeePayment(sut.RequiredMembershipFee,rejectionDate);
        var reason = this.fixture.Create<string>();
        var deadline = this.fixture.Create<DateTime>();
        sut.RejectApplication(rejectionDate,reason,deadline);
        
        // Act
        sut.AppealRejection(deadline.AddDays(-2),reason);
        
        // Assert
        sut.Recommendations.Should().HaveCount(1);
        sut.Recommendations.All(x=>x.IsEndorsed).Should().BeTrue();
        sut.RejectionReason.Should().Be(reason);
        sut.AppealDeadline.Should().Be(deadline);
        sut.AppealDate.Should().Be(deadline.AddDays(-2));
        sut.AppealReason.Should().Be(reason);
        sut.Status.Should().Be(ApplicationStatus.RejectionAppealed);
        sut.GetChangeEvents().Should().HaveCount(7);
        sut.GetChangeEvents().Should().ContainItemsAssignableTo<ApplicationRejectionAppealReceivedChangeEvent>();
        sut.GetDomainEvents().Should().HaveCount(6);
        sut.GetDomainEvents().Should().ContainItemsAssignableTo<ApplicationRejectionAppealReceivedDomainEvent>();
    }
    
    [Fact]
    public void ApproveAppeal_should_emmit_ApplicationRejectionAppealApprovedChangeEvent()
    {
        // Arrange
        var sut = this.CreateValidApplicationForm();
        var requiredFee = this.fixture.Create<decimal>();
        sut.AcceptApplication(Money.FromPln(requiredFee));
        sut.RequestRecommendation(this.fixture.Create<DateTime>());
        sut.EndorseApplication(sut.Recommendations.First().Id);
        var rejectionDate = this.fixture.Create<DateTime>();
        sut.RegisterMembershipFeePayment(sut.RequiredMembershipFee,rejectionDate);
        var reason = this.fixture.Create<string>();
        var deadline = this.fixture.Create<DateTime>();
        sut.RejectApplication(rejectionDate,reason,deadline);
        sut.AppealRejection(deadline.AddDays(-2),reason);
        var decisionDate = this.fixture.Create<DateTime>();
        // Act
        sut.ApproveAppeal(decisionDate,reason);
        
        // Assert
        sut.Recommendations.Should().HaveCount(1);
        sut.Recommendations.All(x=>x.IsEndorsed).Should().BeTrue();
        sut.RejectionReason.Should().Be(reason);
        sut.AppealDeadline.Should().Be(deadline);
        sut.AppealDate.Should().Be(deadline.AddDays(-2));
        sut.AppealReason.Should().Be(reason);
        sut.FinalDecisionDate.Should().Be(decisionDate);
        sut.Status.Should().Be(ApplicationStatus.AppealApproved);
        sut.GetChangeEvents().Should().HaveCount(8);
        sut.GetChangeEvents().Should().ContainItemsAssignableTo<ApplicationRejectionAppealApprovedChangeEvent>();
        sut.GetDomainEvents().Should().HaveCount(7);
        sut.GetDomainEvents().Should().ContainItemsAssignableTo<ApplicationRejectionAppealApprovedDomainEvent>();
    }
    
    [Fact]
    public void RejectAppeal_should_emmit_ApplicationRejectionAppealRejectedChangeEvent()
    {
        // Arrange
        var sut = this.CreateValidApplicationForm();
        var requiredFee = this.fixture.Create<decimal>();
        sut.AcceptApplication(Money.FromPln(requiredFee));
        sut.RequestRecommendation(this.fixture.Create<DateTime>());
        sut.EndorseApplication(sut.Recommendations.First().Id);
        var rejectionDate = this.fixture.Create<DateTime>();
        sut.RegisterMembershipFeePayment(sut.RequiredMembershipFee,rejectionDate);
        var reason = this.fixture.Create<string>();
        var deadline = this.fixture.Create<DateTime>();
        sut.RejectApplication(rejectionDate,reason,deadline);
        sut.AppealRejection(deadline.AddDays(-2),reason);
        var decisionDate = this.fixture.Create<DateTime>();
        // Act
        sut.RejectAppeal(decisionDate,reason);
        
        // Assert
        sut.Recommendations.Should().HaveCount(1);
        sut.Recommendations.All(x=>x.IsEndorsed).Should().BeTrue();
        sut.RejectionReason.Should().Be(reason);
        sut.AppealDeadline.Should().Be(deadline);
        sut.AppealDate.Should().Be(deadline.AddDays(-2));
        sut.AppealReason.Should().Be(reason);
        sut.FinalDecisionDate.Should().Be(decisionDate);
        sut.FinalDecisionReason.Should().Be(reason);
        sut.Status.Should().Be(ApplicationStatus.AppealRejected);
        sut.GetChangeEvents().Should().HaveCount(8);
        sut.GetChangeEvents().Should().ContainItemsAssignableTo<ApplicationRejectionAppealRejectedChangeEvent>();
        sut.GetDomainEvents().Should().HaveCount(7);
        sut.GetDomainEvents().Should().ContainItemsAssignableTo<ApplicationRejectionAppealRejectedDomainEvent>();
    }
    
    [Fact]
    public void CancelApplication_should_emmit_ApplicationCancelledChangeEvent()
    {
        // Arrange
        var sut = this.CreateValidApplicationForm();
        var cancellationDate = this.fixture.Create<DateTime>();
        
        // Act
        sut.CancelApplication(cancellationDate);
        
        // Assert
        sut.Status.Should().Be(ApplicationStatus.Cancelled);
        sut.Recommendations.Should().HaveCount(1);
        sut.Recommendations.All(x=>!x.IsValid).Should().BeTrue();
        sut.GetChangeEvents().Should().HaveCount(1);
        sut.GetChangeEvents().Should().ContainItemsAssignableTo<ApplicationCancelledChangeEvent>();
        sut.GetDomainEvents().Should().HaveCount(1);
        sut.GetDomainEvents().Should().ContainItemsAssignableTo<ApplicationCancelledDomainEvent>();
    }
    
    private Application CreateValidApplicationForm()
    {
        var firstName = this.fixture.Create<string>();
        var lastName = this.fixture.Create<string>();
        var addressCountry = this.fixture.Create<string>();
        var addressCity = this.fixture.Create<string>();
        var addressStreet = this.fixture.Create<string>();
        var addressNumber = this.fixture.Create<string>();
        var address = new Address(addressCountry, addressCity, addressNumber, addressStreet);
        var (birthDate, _) = this.fixture.Create<DateTime>();
        var email = this.fixture.Create<string>();
        var phoneNumber = this.fixture.Create<string>();
        var applicationDate = this.fixture.Create<DateTime>();
        var recommender = this.fixture.Create<string>();
        var recommendation = new ApplicationRecommendation(Guid.NewGuid(),
            new CardNumber(this.fixture.Create<string>(), this.fixture.Create<int>()));
        
        var applicationForm=  new Application(firstName,
            lastName,
            address,
            birthDate,
            email,
            phoneNumber,
            new List<ApplicationRecommendation>()
            {
                recommendation
            },
            applicationDate);
        applicationForm.ClearChangeEvents();
        applicationForm.ClearDomainEvents();
        return applicationForm;
    }
}