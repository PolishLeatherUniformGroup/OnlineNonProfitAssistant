using AutoFixture;
using FluentAssertions;
using PLUG.ONPA.Apply.Domain.ChangeEvents;
using PLUG.ONPA.Apply.Domain.DomainEvents;
using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.UnitTests;

public class ApplicationFormTests
{
    private readonly IFixture fixture;

    public ApplicationFormTests()
    {
        this.fixture = new Fixture();
    }
    
    [Fact]
    public void CreateApplicationForm_should_emmit_ApplicationFormCreatedChangeEvent()
    {
        // Arrange
        var firstName = fixture.Create<string>();
        var lastName = fixture.Create<string>();
        var addressCountry = fixture.Create<string>();
        var addressCity = fixture.Create<string>();
        var addressStreet = fixture.Create<string>();
        var addressNumber = fixture.Create<string>();
        var address = new Address(addressCountry, addressCity, addressNumber, addressStreet);
        var (birthDate, _) = fixture.Create<DateTime>();
        var email = fixture.Create<string>();
        var phoneNumber = fixture.Create<string>();
        var applicationDate = fixture.Create<DateTime>();
        var recommender = fixture.Create<string>();
        var recommendation = new ApplicationRecommendation(Guid.NewGuid(),
            new CardNumber(this.fixture.Create<string>(), this.fixture.Create<int>()));
        
        // Act
        var sut = new ApplicationForm(firstName,
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
        sut.GetChangeEvents().Should().ContainItemsAssignableTo<ApplicationFormCreatedChangeEvent>();
        sut.GetDomainEvents().Should().HaveCount(1);
        sut.GetDomainEvents().Should().ContainItemsAssignableTo<ApplicationFormReceivedDomainEvent>();
    }
    
    [Fact]
    public void AcceptApplicationForm_should_emmit_ApplicationAcceptedChangeEvent()
    {
        // Arrange
        var sut = CreateValidApplicationForm();
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
        var sut = CreateValidApplicationForm();
        
        
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
        var sut = CreateValidApplicationForm();
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
        var sut = CreateValidApplicationForm();
        var requiredFee = this.fixture.Create<decimal>();
        sut.AcceptApplication(Money.FromPln(requiredFee));
        sut.RequestRecommendation(this.fixture.Create<DateTime>());
        
        // Act
        sut.EndorseApplication(sut.Recommendations.First().Id);
        
        // Assert
        sut.Recommendations.Should().HaveCount(1);
        sut.Recommendations.All(x=>x.IsEndorsed).Should().BeTrue();
        sut.Status.Should().Be(ApplicationStatus.AwaitsDecision);
        sut.GetChangeEvents().Should().HaveCount(3);
        sut.GetChangeEvents().Should().ContainItemsAssignableTo<ApplicationEndorsedChangeEvent>();
        sut.GetDomainEvents().Should().HaveCount(3);
        sut.GetDomainEvents().Should().ContainItemsAssignableTo<ApplicationEndorsedDomainEvent>();
    }
    
    [Fact]
    public void EndorseApplication_should_throwException_when_recommendtionNotFound()
    {
        // Arrange
        var sut = CreateValidApplicationForm();
        var requiredFee = this.fixture.Create<decimal>();
        sut.AcceptApplication(Money.FromPln(requiredFee));
        sut.RequestRecommendation(this.fixture.Create<DateTime>());
        
        // Act
       var result = ()=> sut.EndorseApplication(Guid.NewGuid());
        
        // Assert
       result.Should().Throw<InvalidOperationException>();
       
    }
    
    [Fact]
    public void OpposeApplication_should_emmit_ApplicationEndorsedChangeEvent()
    {
        // Arrange
        var sut = CreateValidApplicationForm();
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
        var sut = CreateValidApplicationForm();
        var requiredFee = this.fixture.Create<decimal>();
        sut.AcceptApplication(Money.FromPln(requiredFee));
        sut.RequestRecommendation(this.fixture.Create<DateTime>());
        
        // Act
        var result = ()=> sut.OpposeApplication(Guid.NewGuid());
        
        // Assert
        result.Should().Throw<InvalidOperationException>();
       
    }
    
    [Fact]
    public void RegisterMembershipFeePayment_should_emmit_ApplicationMembershipFeePaymentReceivedChangeEvent()
    {
        // Arrange
        var sut = CreateValidApplicationForm();
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
        var sut = CreateValidApplicationForm();
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
        sut.GetChangeEvents().Should().HaveCount(5);
        sut.GetChangeEvents().Should().ContainItemsAssignableTo<ApplicationApprovedChangeEvent>();
        sut.GetDomainEvents().Should().HaveCount(5);
        sut.GetDomainEvents().Should().ContainItemsAssignableTo<ApplicationApprovedDomainEvent>();
    }
    
    [Fact]
    public void RejectApplication_should_emmit_ApplicationRejectedChangeEvent()
    {
        // Arrange
        var sut = CreateValidApplicationForm();
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
        sut.GetChangeEvents().Should().HaveCount(5);
        sut.GetChangeEvents().Should().ContainItemsAssignableTo<ApplicationRejectedChangeEvent>();
        sut.GetDomainEvents().Should().HaveCount(5);
        sut.GetDomainEvents().Should().ContainItemsAssignableTo<ApplicationRejectedDomainEvent>();
    }
    
    private ApplicationForm CreateValidApplicationForm()
    {
        var firstName = fixture.Create<string>();
        var lastName = fixture.Create<string>();
        var addressCountry = fixture.Create<string>();
        var addressCity = fixture.Create<string>();
        var addressStreet = fixture.Create<string>();
        var addressNumber = fixture.Create<string>();
        var address = new Address(addressCountry, addressCity, addressNumber, addressStreet);
        var (birthDate, _) = fixture.Create<DateTime>();
        var email = fixture.Create<string>();
        var phoneNumber = fixture.Create<string>();
        var applicationDate = fixture.Create<DateTime>();
        var recommender = fixture.Create<string>();
        var recommendation = new ApplicationRecommendation(Guid.NewGuid(),
            new CardNumber(this.fixture.Create<string>(), this.fixture.Create<int>()));
        
        var applicationForm=  new ApplicationForm(firstName,
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