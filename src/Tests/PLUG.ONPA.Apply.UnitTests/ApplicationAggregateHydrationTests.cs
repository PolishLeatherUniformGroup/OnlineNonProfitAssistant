using AutoFixture;
using FluentAssertions;
using PLUG.ONPA.Apply.Domain.ChangeEvents;
using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Apply.UnitTests.Helpers;
using PLUG.ONPA.Common.Domain.Abstractions;

namespace PLUG.ONPA.Apply.UnitTests;

public class ApplicationAggregateHydrationTests
{
    private IFixture fixture;
    
    public ApplicationAggregateHydrationTests()
    {
        fixture = new Fixture().Customize(new CompositeCustomization(
            new DateOnlyFixtureCustomization()));
    }

    [Fact]
    public void ApplicationAggregate_Should_ApplyApplicationCreatedChangeEvent()
    {
        // Arrange
        var aggregateId = fixture.Create<Guid>();
        var application = new ApplicationAggregate(aggregateId, Enumerable.Empty<IChangeEvent>(), null);
        var applicationCreatedEvent = fixture.Create<ApplicationCreatedChangeEvent>();
        
        // Act
        application.ApplyChange(applicationCreatedEvent);
        
        // Assert
        application.FirstName.Value.Should().Be(applicationCreatedEvent.FirstName);
        application.LastName.Value.Should().Be(applicationCreatedEvent.LastName);
        application.Address.Should().Be(applicationCreatedEvent.Address);
        application.BirthDate.Should().Be(applicationCreatedEvent.BirthDate);
        application.Email.Value.Should().Be(applicationCreatedEvent.Email);
        application.PhoneNumber.Value.Should().Be(applicationCreatedEvent.PhoneNumber);
        application.Status.Should().Be(applicationCreatedEvent.Status);
        application.ApplicationDate.Should().Be(applicationCreatedEvent.ApplicationDate);
        application.Recommendations.Should().BeEquivalentTo(applicationCreatedEvent.Recommendations);
        
    }
    
    [Fact]
    public void ApplicationAggregate_Should_ApplyApplicationAcceptedChangeEvent()
    {
        // Arrange
        var aggregateId = fixture.Create<Guid>();
        var application = new ApplicationAggregate(aggregateId, Enumerable.Empty<IChangeEvent>(), null);
        var applicationCreatedEvent = fixture.Create<ApplicationCreatedChangeEvent>();
        application.ApplyChange(applicationCreatedEvent);
        
        var applicationAcceptedEvent = fixture.Create<ApplicationAcceptedChangeEvent>();
        // Act
        application.ApplyChange(applicationAcceptedEvent);
        
        // Assert
        application.RequiredMembershipFee.Should().Be(applicationAcceptedEvent.RequiredFee);
        application.Status.Should().Be(applicationAcceptedEvent.Status);
    }
    
    [Fact]
    public void ApplicationAggregate_Should_ApplyApplicationDismissedChangeEvent()
    {
        // Arrange
        var aggregateId = fixture.Create<Guid>();
        var application = new ApplicationAggregate(aggregateId, Enumerable.Empty<IChangeEvent>(), null);
        var applicationCreatedEvent = fixture.Create<ApplicationCreatedChangeEvent>();
        application.ApplyChange(applicationCreatedEvent);
        
        var applicationAcceptedEvent = fixture.Create<ApplicationDismissedChangeEvent>();
        // Act
        application.ApplyChange(applicationAcceptedEvent);
        
        // Assert
        application.Status.Should().Be(applicationAcceptedEvent.Status);
    }
    
    [Fact]
    public void ApplicationAggregate_Should_ApplyApplicationCancelledChangeEvent()
    {
        // Arrange
        var aggregateId = fixture.Create<Guid>();
        var application = new ApplicationAggregate(aggregateId, Enumerable.Empty<IChangeEvent>(), null);
        var applicationCreatedEvent = fixture.Create<ApplicationCreatedChangeEvent>();
        application.ApplyChange(applicationCreatedEvent);
        
        var applicationAcceptedEvent = fixture.Create<ApplicationCancelledChangeEvent>();
        // Act
        application.ApplyChange(applicationAcceptedEvent);
        
        // Assert
        application.Status.Should().Be(applicationAcceptedEvent.Status);
    }
    
    [Fact]
    public void ApplicationAggregate_Should_ApplyApplicationRecomendationRequestedChangeEvent()
    {
        // Arrange
        var aggregateId = fixture.Create<Guid>();
        var application = new ApplicationAggregate(aggregateId, Enumerable.Empty<IChangeEvent>(), null);
        var applicationCreatedEvent = fixture.Create<ApplicationCreatedChangeEvent>();
        application.ApplyChange(applicationCreatedEvent);
        var applicationAcceptedEvent = fixture.Create<ApplicationAcceptedChangeEvent>();
        application.ApplyChange(applicationAcceptedEvent);
        
        var requestRecommendationEvent = fixture.Create<ApplicationRecommendationRequestedChangeEvent>();
        
        // Act
        application.ApplyChange(requestRecommendationEvent);
        
        // Assert
        application.Recommendations.All(x=>x.RequestedAt == requestRecommendationEvent.RequestedAt).Should().BeTrue();
    }
    
    [Fact]
    public void ApplicationAggregate_Should_ApplyApplicationEndorsedChangeEvent()
    {
        // Arrange
        var aggregateId = fixture.Create<Guid>();
        var application = new ApplicationAggregate(aggregateId, Enumerable.Empty<IChangeEvent>(), null);
        var applicationCreatedEvent = fixture.Create<ApplicationCreatedChangeEvent>();
        application.ApplyChange(applicationCreatedEvent);
        var applicationAcceptedEvent = fixture.Create<ApplicationAcceptedChangeEvent>();
        application.ApplyChange(applicationAcceptedEvent);
        var requestRecommendationEvent = fixture.Create<ApplicationRecommendationRequestedChangeEvent>();
        application.ApplyChange(requestRecommendationEvent);
        var applicationEndorsedEvent = new ApplicationEndorsedChangeEvent(application.Recommendations.First().Id);
        
        // Act
        application.ApplyChange(applicationEndorsedEvent);
        
        // Assert
        application.Recommendations.First().IsEndorsed.Should().BeTrue();
    }
    
    [Fact]
    public void ApplicationAggregate_Should_ApplyApplicationOpposedChangeEvent()
    {
        // Arrange
        var aggregateId = fixture.Create<Guid>();
        var application = new ApplicationAggregate(aggregateId, Enumerable.Empty<IChangeEvent>(), null);
        var applicationCreatedEvent = fixture.Create<ApplicationCreatedChangeEvent>();
        application.ApplyChange(applicationCreatedEvent);
        var applicationAcceptedEvent = fixture.Create<ApplicationAcceptedChangeEvent>();
        application.ApplyChange(applicationAcceptedEvent);
        var requestRecommendationEvent = fixture.Create<ApplicationRecommendationRequestedChangeEvent>();
        application.ApplyChange(requestRecommendationEvent);
        var applicationEndorsedEvent = new ApplicationOpposedChangeEvent(ApplicationStatus.Rejected, application.Recommendations.First().Id);
        
        // Act
        application.ApplyChange(applicationEndorsedEvent);
        
        // Assert
        application.Recommendations.First().IsRefused.Should().BeTrue();
    }
    
    [Fact]
    public void ApplicationAggregate_Should_ApplyApplicationApprovedChangeEvent()
    {
        // Arrange
        var aggregateId = fixture.Create<Guid>();
        var application = new ApplicationAggregate(aggregateId, Enumerable.Empty<IChangeEvent>(), null);
        var applicationCreatedEvent = fixture.Create<ApplicationCreatedChangeEvent>();
        application.ApplyChange(applicationCreatedEvent);
        var applicationAcceptedEvent = fixture.Create<ApplicationAcceptedChangeEvent>();
        application.ApplyChange(applicationAcceptedEvent);
        var requestRecommendationEvent = fixture.Create<ApplicationRecommendationRequestedChangeEvent>();
        application.ApplyChange(requestRecommendationEvent);
        var applicationApproved = this.fixture.Create<ApplicationApprovedChangeEvent>();
        // Act
        application.ApplyChange(applicationApproved);
        
        // Assert
        application.Status.Should().Be(applicationApproved.Status);
        application.FirstDecisionDate.Should().Be(applicationApproved.DecisionDate);
    }
    
    [Fact]
    public void ApplicationAggregate_Should_ApplyApplicationRejectedChangeEvent()
    {
        // Arrange
        var aggregateId = fixture.Create<Guid>();
        var application = new ApplicationAggregate(aggregateId, Enumerable.Empty<IChangeEvent>(), null);
        var applicationCreatedEvent = fixture.Create<ApplicationCreatedChangeEvent>();
        application.ApplyChange(applicationCreatedEvent);
        var applicationAcceptedEvent = fixture.Create<ApplicationAcceptedChangeEvent>();
        application.ApplyChange(applicationAcceptedEvent);
        var requestRecommendationEvent = fixture.Create<ApplicationRecommendationRequestedChangeEvent>();
        application.ApplyChange(requestRecommendationEvent);
        var applicationApproved = this.fixture.Create<ApplicationRejectedChangeEvent>();
        // Act
        application.ApplyChange(applicationApproved);
        
        // Assert
        application.Status.Should().Be(applicationApproved.Status);
        application.FirstDecisionDate.Should().Be(applicationApproved.DecisionDate);
        application.RejectionReason.Should().Be(applicationApproved.DecisionReason);
        application.AppealDeadline.Should().Be(applicationApproved.AppealDeadline);
    }
    
    [Fact]
    public void ApplicationAggregate_Should_ApplyApplicationRejectionAppealedChangeEvent()
    {
        // Arrange
        var aggregateId = fixture.Create<Guid>();
        var application = new ApplicationAggregate(aggregateId, Enumerable.Empty<IChangeEvent>(), null);
        var applicationCreatedEvent = fixture.Create<ApplicationCreatedChangeEvent>();
        application.ApplyChange(applicationCreatedEvent);
        var applicationAcceptedEvent = fixture.Create<ApplicationAcceptedChangeEvent>();
        application.ApplyChange(applicationAcceptedEvent);
        var requestRecommendationEvent = fixture.Create<ApplicationRecommendationRequestedChangeEvent>();
        application.ApplyChange(requestRecommendationEvent);
        var applicationApproved = this.fixture.Create<ApplicationRejectionAppealReceivedChangeEvent>();
        // Act
        application.ApplyChange(applicationApproved);
        
        // Assert
        application.Status.Should().Be(applicationApproved.Status);
        application.AppealDate.Should().Be(applicationApproved.AppealDate);
        application.AppealReason.Should().Be(applicationApproved.AppealReason);
    }
    
    [Fact]
    public void ApplicationAggregate_Should_ApplyApplicationRejectionAppealDismissedChangeEvent()
    {
        // Arrange
        var aggregateId = fixture.Create<Guid>();
        var application = new ApplicationAggregate(aggregateId, Enumerable.Empty<IChangeEvent>(), null);
        var applicationCreatedEvent = fixture.Create<ApplicationCreatedChangeEvent>();
        application.ApplyChange(applicationCreatedEvent);
        var applicationAcceptedEvent = fixture.Create<ApplicationAcceptedChangeEvent>();
        application.ApplyChange(applicationAcceptedEvent);
        var requestRecommendationEvent = fixture.Create<ApplicationRecommendationRequestedChangeEvent>();
        application.ApplyChange(requestRecommendationEvent);
        var applicationApproved = this.fixture.Create<ApplicationRejectionAppealDismissedChangeEvent>();
        // Act
        application.ApplyChange(applicationApproved);
        
        // Assert
        application.Status.Should().Be(applicationApproved.Status);
        application.AppealDate.Should().Be(applicationApproved.AppealDate);
        application.FinalDecisionDate.Should().Be(applicationApproved.AppealDate);
    }
}