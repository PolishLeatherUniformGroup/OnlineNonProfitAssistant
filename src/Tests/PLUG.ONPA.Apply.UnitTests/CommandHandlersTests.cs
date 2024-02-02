using AutoFixture;
using FluentAssertions;
using NSubstitute;
using PLUG.ONPA.Apply.Api.CommandHandlers;
using PLUG.ONPA.Apply.Api.Commands;
using PLUG.ONPA.Apply.Api.Services;
using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Apply.UnitTests.Helpers;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.UnitTests;

public class CommandHandlersTests
{
    private IFixture fixture;
    private IAggregateRepository<Domain.Model.ApplicationAggregate> aggregateRepository;
    private ITenantSettingsService tenantSettingsService;
    
    public CommandHandlersTests()
    {
        this.fixture = new Fixture().Customize(new CompositeCustomization(
            new DateOnlyFixtureCustomization()));
        this.aggregateRepository = Substitute.For<IAggregateRepository<Domain.Model.ApplicationAggregate>>();
        this.tenantSettingsService = Substitute.For<ITenantSettingsService>();
    }
    
    [Fact]
    public void CreateApplicationCommandHandler_Handle()
    {
        // Arrange
        var cardNumber = this.fixture.Create<int>();
        var cardPrefix = this.fixture.Create<string>();
        var card = new CardNumber(cardPrefix, cardNumber);
        var command = this.fixture.Build<CreateApplicationCommand>()
            .With(x=>x.Recommendations, new List<string>(){card})
            .Create();
        var handler = new CreateApplicationCommandHandler(this.aggregateRepository);
        
        // Act
        var result = handler.Handle(command, CancellationToken.None).Result;
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeFalse();
        aggregateRepository.Received(1).SaveAsync(Arg.Any<Domain.Model.ApplicationAggregate>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task AcceptApplicationCommandHandler_Handle()
    {
        // Arrange
        var application = CreateValidApplicationForm();
        var command = this.fixture.Build<AcceptApplicationCommand>()
            .With(x=>x.ApplicationId, application.AggregateId)
            .With(x=>x.TenantId, application.TenantId)
            .Create();
        var handler = new AcceptApplicationCommandHandler(this.aggregateRepository);
        aggregateRepository.GetByIdAsync(application.AggregateId, application.TenantId, CancellationToken.None)
            .Returns(application);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeFalse();
        application.Status.Should().Be(ApplicationStatus.Valid);
        await aggregateRepository.Received(1).GetByIdAsync(application.AggregateId, application.TenantId, Arg.Any<CancellationToken>());
        await aggregateRepository.Received(1).SaveAsync(application, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task AcceptApplicationCommandHandler_Fail()
    {
        // Arrange
        var application = CreateValidApplicationForm();
        var command = this.fixture.Build<AcceptApplicationCommand>()
            .With(x=>x.ApplicationId, application.AggregateId)
            .With(x=>x.TenantId, application.TenantId)
            .Create();
        var handler = new AcceptApplicationCommandHandler(this.aggregateRepository);
        aggregateRepository.GetByIdAsync(application.AggregateId, application.TenantId, CancellationToken.None)
            .Returns(null as Domain.Model.ApplicationAggregate);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeTrue();
        application.Status.Should().Be(ApplicationStatus.Received);
        await aggregateRepository.Received(1).GetByIdAsync(application.AggregateId, application.TenantId, Arg.Any<CancellationToken>());
        await aggregateRepository.Received(0).SaveAsync(application, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task RequestRecommendationCommandHandler_Handle()
    {
        // Arrange
        var application = CreateValidApplicationForm();
        application.AcceptApplication(Money.FromPln(this.fixture.Create<decimal>()));
        var command = this.fixture.Build<RequestRecommendationCommand>()
            .With(x=>x.ApplicationId, application.AggregateId)
            .With(x=>x.TenantId, application.TenantId)
            .Create();
        var handler = new RequestRecommendationCommandHandler(this.aggregateRepository);
        aggregateRepository.GetByIdAsync(application.AggregateId, application.TenantId, CancellationToken.None)
            .Returns(application);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeFalse();
        application.Status.Should().Be(ApplicationStatus.InRecommendation);
        await aggregateRepository.Received(1).GetByIdAsync(application.AggregateId, application.TenantId, Arg.Any<CancellationToken>());
        await aggregateRepository.Received(1).SaveAsync(application, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task RequestRecommendationCommandHandler_Fail()
    {
        // Arrange
        var application = CreateValidApplicationForm();
        application.AcceptApplication(Money.FromPln(this.fixture.Create<decimal>()));
        var command = this.fixture.Build<RequestRecommendationCommand>()
            .With(x=>x.ApplicationId, application.AggregateId)
            .With(x=>x.TenantId, application.TenantId)
            .Create();
        var handler = new RequestRecommendationCommandHandler(this.aggregateRepository);
        aggregateRepository.GetByIdAsync(application.AggregateId, application.TenantId, CancellationToken.None)
            .Returns(null as Domain.Model.ApplicationAggregate);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeTrue();
        application.Status.Should().Be(ApplicationStatus.Valid);
        await aggregateRepository.Received(1).GetByIdAsync(application.AggregateId, application.TenantId, Arg.Any<CancellationToken>());
        await aggregateRepository.Received(0).SaveAsync(application, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task DismissApplicationCommandHandler_Handle()
    {
        // Arrange
        var application = CreateValidApplicationForm();
        var command = this.fixture.Build<DismissApplicationCommand>()
            .With(x=>x.ApplicationId, application.AggregateId)
            .With(x=>x.TenantId, application.TenantId)
            .With(x=>x.ValidRecommendations, new List<string>())
            .Create();
        var handler = new DismissApplicationCommandHandler(this.aggregateRepository);
        aggregateRepository.GetByIdAsync(application.AggregateId, application.TenantId, CancellationToken.None)
            .Returns(application);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeFalse();
        application.Status.Should().Be(ApplicationStatus.Invalid);
        await aggregateRepository.Received(1).GetByIdAsync(application.AggregateId, application.TenantId, Arg.Any<CancellationToken>());
        await aggregateRepository.Received(1).SaveAsync(application, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task DismissApplicationCommandHandler_Fail()
    {
        // Arrange
        var application = CreateValidApplicationForm();
        var command = this.fixture.Build<DismissApplicationCommand>()
            .With(x=>x.ApplicationId, application.AggregateId)
            .With(x=>x.TenantId, application.TenantId)
            .With(x=>x.ValidRecommendations, new List<string>())
            .Create();
        var handler = new DismissApplicationCommandHandler(this.aggregateRepository);
        aggregateRepository.GetByIdAsync(application.AggregateId, application.TenantId, CancellationToken.None)
            .Returns(null as Domain.Model.ApplicationAggregate);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeTrue();
        application.Status.Should().Be(ApplicationStatus.Received);
        await aggregateRepository.Received(1).GetByIdAsync(application.AggregateId, application.TenantId, Arg.Any<CancellationToken>());
        await aggregateRepository.Received(0).SaveAsync(application, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task EndorseApplicationCommandHandler_Handle()
    {
        // Arrange
        var application = CreateValidApplicationForm();
        application.AcceptApplication(Money.FromPln(this.fixture.Create<decimal>()));
        application.RequestRecommendation(this.fixture.Create<DateTime>());
        var command = this.fixture.Build<EndorseApplicationCommand>()
            .With(x=>x.ApplicationId, application.AggregateId)
            .With(x=>x.TenantId, application.TenantId)
            .With(x=>x.RecommendationId, application.Recommendations.First().Id)
            .Create();
        var handler = new EndorseApplicationCommandHandler(this.aggregateRepository);
        aggregateRepository.GetByIdAsync(application.AggregateId, application.TenantId, CancellationToken.None)
            .Returns(application);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeFalse();
        application.Status.Should().Be(ApplicationStatus.AwaitsDecision);
        await aggregateRepository.Received(1).GetByIdAsync(application.AggregateId, application.TenantId, Arg.Any<CancellationToken>());
        await aggregateRepository.Received(1).SaveAsync(application, Arg.Any<CancellationToken>());
    }
    [Fact]
    public async Task EndorseApplicationCommandHandler_Fail()
    {
        // Arrange
        var application = CreateValidApplicationForm();
        application.AcceptApplication(Money.FromPln(this.fixture.Create<decimal>()));
        application.RequestRecommendation(this.fixture.Create<DateTime>());
        var command = this.fixture.Build<EndorseApplicationCommand>()
            .With(x=>x.ApplicationId, application.AggregateId)
            .With(x=>x.TenantId, application.TenantId)
            .With(x=>x.RecommendationId, application.Recommendations.First().Id)
            .Create();
        var handler = new EndorseApplicationCommandHandler(this.aggregateRepository);
        aggregateRepository.GetByIdAsync(application.AggregateId, application.TenantId, CancellationToken.None)
            .Returns(null as Domain.Model.ApplicationAggregate);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeTrue();
        application.Status.Should().Be(ApplicationStatus.InRecommendation);
        await aggregateRepository.Received(1).GetByIdAsync(application.AggregateId, application.TenantId, Arg.Any<CancellationToken>());
        await aggregateRepository.Received(0).SaveAsync(application, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task OpposeApplicationCommandHandler_Handle()
    {
        // Arrange
        var application = CreateValidApplicationForm();
        application.AcceptApplication(Money.FromPln(this.fixture.Create<decimal>()));
        application.RequestRecommendation(this.fixture.Create<DateTime>());
        var command = this.fixture.Build<OpposeApplicationCommand>()
            .With(x=>x.ApplicationId, application.AggregateId)
            .With(x=>x.TenantId, application.TenantId)
            .With(x=>x.RecommendationId, application.Recommendations.First().Id)
            .Create();
        var handler = new OpposeApplicationCommandHandler(this.aggregateRepository);
        aggregateRepository.GetByIdAsync(application.AggregateId, application.TenantId, CancellationToken.None)
            .Returns(application);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeFalse();
        application.Status.Should().Be(ApplicationStatus.Rejected);
        await aggregateRepository.Received(1).GetByIdAsync(application.AggregateId, application.TenantId, Arg.Any<CancellationToken>());
        await aggregateRepository.Received(1).SaveAsync(application, Arg.Any<CancellationToken>());
    }
    [Fact]
    public async Task OpposeApplicationCommandHandler_Fail()
    {
        // Arrange
        var application = CreateValidApplicationForm();
        application.AcceptApplication(Money.FromPln(this.fixture.Create<decimal>()));
        application.RequestRecommendation(this.fixture.Create<DateTime>());
        var command = this.fixture.Build<OpposeApplicationCommand>()
            .With(x=>x.ApplicationId, application.AggregateId)
            .With(x=>x.TenantId, application.TenantId)
            .With(x=>x.RecommendationId, application.Recommendations.First().Id)
            .Create();
        var handler = new OpposeApplicationCommandHandler(this.aggregateRepository);
        aggregateRepository.GetByIdAsync(application.AggregateId, application.TenantId, CancellationToken.None)
            .Returns(null as Domain.Model.ApplicationAggregate);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeTrue();
        application.Status.Should().Be(ApplicationStatus.InRecommendation);
        await aggregateRepository.Received(1).GetByIdAsync(application.AggregateId, application.TenantId, Arg.Any<CancellationToken>());
        await aggregateRepository.Received(0).SaveAsync(application, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task RegisterApplicationFeeCommandHandler_Handle()
    {
        // Arrange
        var application = CreateValidApplicationForm();
        application.AcceptApplication(Money.FromPln(this.fixture.Create<decimal>()));
        application.RequestRecommendation(this.fixture.Create<DateTime>());
        application.EndorseApplication(application.Recommendations.First().Id);
        var command = this.fixture.Build<RegisterApplicationFeePaymentCommand>()
            .With(x=>x.ApplicationId, application.AggregateId)
            .With(x=>x.TenantId, application.TenantId)
            .Create();
        var handler = new RegisterApplicationFeePaymentCommandHandler(this.aggregateRepository);
        aggregateRepository.GetByIdAsync(application.AggregateId, application.TenantId, CancellationToken.None)
            .Returns(application);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeFalse();
        application.Status.Should().Be(ApplicationStatus.AwaitsDecision);
        await aggregateRepository.Received(1).GetByIdAsync(application.AggregateId, application.TenantId, Arg.Any<CancellationToken>());
        await aggregateRepository.Received(1).SaveAsync(application, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task RegisterApplicationFeeCommandHandler_Fail()
    {
        // Arrange
        var application = CreateValidApplicationForm();
        application.AcceptApplication(Money.FromPln(this.fixture.Create<decimal>()));
        application.RequestRecommendation(this.fixture.Create<DateTime>());
        application.EndorseApplication(application.Recommendations.First().Id);
        var command = this.fixture.Build<RegisterApplicationFeePaymentCommand>()
            .With(x=>x.ApplicationId, application.AggregateId)
            .With(x=>x.TenantId, application.TenantId)
            .Create();
        var handler = new RegisterApplicationFeePaymentCommandHandler(this.aggregateRepository);
        aggregateRepository.GetByIdAsync(application.AggregateId, application.TenantId, CancellationToken.None)
            .Returns(null as Domain.Model.ApplicationAggregate);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeTrue();
        application.Status.Should().Be(ApplicationStatus.AwaitsDecision);
        await aggregateRepository.Received(1).GetByIdAsync(application.AggregateId, application.TenantId, Arg.Any<CancellationToken>());
        await aggregateRepository.Received(0).SaveAsync(application, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task ApproveApplicationCommandHandler_Handle()
    {
        // Arrange
        var application = CreateValidApplicationForm();
        application.AcceptApplication(Money.FromPln(this.fixture.Create<decimal>()));
        application.RequestRecommendation(this.fixture.Create<DateTime>());
        application.EndorseApplication(application.Recommendations.First().Id);
        application.RegisterMembershipFeePayment(application.RequiredMembershipFee,this.fixture.Create<DateTime>());
        var command = this.fixture.Build<ApproveApplicationCommand>()
            .With(x=>x.ApplicationId, application.AggregateId)
            .With(x=>x.TenantId, application.TenantId)
            .Create();
        var handler = new ApproveApplicationCommandHandler(this.aggregateRepository);
        aggregateRepository.GetByIdAsync(application.AggregateId, application.TenantId, CancellationToken.None)
            .Returns(application);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeFalse();
        application.Status.Should().Be(ApplicationStatus.Approved);
        await aggregateRepository.Received(1).GetByIdAsync(application.AggregateId, application.TenantId, Arg.Any<CancellationToken>());
        await aggregateRepository.Received(1).SaveAsync(application, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task ApproveApplicationCommandHandler_Fail()
    {
        // Arrange
        var application = CreateValidApplicationForm();
        application.AcceptApplication(Money.FromPln(this.fixture.Create<decimal>()));
        application.RequestRecommendation(this.fixture.Create<DateTime>());
        application.EndorseApplication(application.Recommendations.First().Id);
        application.RegisterMembershipFeePayment(application.RequiredMembershipFee,this.fixture.Create<DateTime>());
        var command = this.fixture.Build<ApproveApplicationCommand>()
            .With(x=>x.ApplicationId, application.AggregateId)
            .With(x=>x.TenantId, application.TenantId)
            .Create();
        var handler = new ApproveApplicationCommandHandler(this.aggregateRepository);
        aggregateRepository.GetByIdAsync(application.AggregateId, application.TenantId, CancellationToken.None)
            .Returns(null as Domain.Model.ApplicationAggregate);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeTrue();
        application.Status.Should().Be(ApplicationStatus.AwaitsDecision);
        await aggregateRepository.Received(1).GetByIdAsync(application.AggregateId, application.TenantId, Arg.Any<CancellationToken>());
        await aggregateRepository.Received(0).SaveAsync(application, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task RejectApplicationCommandHandler_Handle()
    {
        // Arrange
        var application = CreateValidApplicationForm();
        application.AcceptApplication(Money.FromPln(this.fixture.Create<decimal>()));
        application.RequestRecommendation(this.fixture.Create<DateTime>());
        application.EndorseApplication(application.Recommendations.First().Id);
        application.RegisterMembershipFeePayment(application.RequiredMembershipFee,this.fixture.Create<DateTime>());
        var command = this.fixture.Build<RejectApplicationCommand>()
            .With(x=>x.ApplicationId, application.AggregateId)
            .With(x=>x.TenantId, application.TenantId)
            .Create();
        this.tenantSettingsService.GetTenantAppealPeriod(application.TenantId!.Value, Arg.Any<CancellationToken>())
            .Returns(TimeSpan.FromDays(14));
        var handler = new RejectApplicationCommandHandler(this.aggregateRepository, this.tenantSettingsService);
        aggregateRepository.GetByIdAsync(application.AggregateId, application.TenantId, CancellationToken.None)
            .Returns(application);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeFalse();
        application.Status.Should().Be(ApplicationStatus.Rejected);
        await aggregateRepository.Received(1).GetByIdAsync(application.AggregateId, application.TenantId, Arg.Any<CancellationToken>());
        await aggregateRepository.Received(1).SaveAsync(application, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task RejectApplicationCommandHandler_Fail()
    {
        // Arrange
        var application = CreateValidApplicationForm();
        application.AcceptApplication(Money.FromPln(this.fixture.Create<decimal>()));
        application.RequestRecommendation(this.fixture.Create<DateTime>());
        application.EndorseApplication(application.Recommendations.First().Id);
        application.RegisterMembershipFeePayment(application.RequiredMembershipFee,this.fixture.Create<DateTime>());
        var command = this.fixture.Build<RejectApplicationCommand>()
            .With(x=>x.ApplicationId, application.AggregateId)
            .With(x=>x.TenantId, application.TenantId)
            .Create();
        this.tenantSettingsService.GetTenantAppealPeriod(application.TenantId!.Value, Arg.Any<CancellationToken>())
            .Returns(TimeSpan.FromDays(14));
        var handler = new RejectApplicationCommandHandler(this.aggregateRepository, this.tenantSettingsService);
        aggregateRepository.GetByIdAsync(application.AggregateId, application.TenantId, CancellationToken.None)
            .Returns(null as Domain.Model.ApplicationAggregate);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeTrue();
        application.Status.Should().Be(ApplicationStatus.AwaitsDecision);
        await aggregateRepository.Received(1).GetByIdAsync(application.AggregateId, application.TenantId, Arg.Any<CancellationToken>());
        await aggregateRepository.Received(0).SaveAsync(application, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task AppealApplicationRejectionCommandHandler_Handle_andDismiss()
    {
        // Arrange
        var application = CreateValidApplicationForm();
        application.AcceptApplication(Money.FromPln(this.fixture.Create<decimal>()));
        application.RequestRecommendation(this.fixture.Create<DateTime>());
        application.EndorseApplication(application.Recommendations.First().Id);
        application.RegisterMembershipFeePayment(application.RequiredMembershipFee,this.fixture.Create<DateTime>());
        this.tenantSettingsService.GetTenantAppealPeriod(application.TenantId!.Value, Arg.Any<CancellationToken>())
            .Returns(TimeSpan.FromDays(14));
        var rejectionDate = this.fixture.Create<DateTime>();
        var appealDeadline = rejectionDate.AddDays(14);
            application.RejectApplication(rejectionDate,this.fixture.Create<string>(),appealDeadline);
        
        var command = this.fixture.Build<AppealApplicationRejectionCommand>()
            .With(x=>x.ApplicationId, application.AggregateId)
            .With(x=>x.TenantId, application.TenantId)
            .With(x=>x.AppealDate, rejectionDate.AddDays(17))
            .Create();
        
        var handler = new AppealApplicationRejectionCommandHandler(this.aggregateRepository);
        aggregateRepository.GetByIdAsync(application.AggregateId, application.TenantId, CancellationToken.None)
            .Returns(application);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeFalse();
        application.Status.Should().Be(ApplicationStatus.AppealDismissed);
        await aggregateRepository.Received(1).GetByIdAsync(application.AggregateId, application.TenantId, Arg.Any<CancellationToken>());
        await aggregateRepository.Received(1).SaveAsync(application, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task AppealApplicationRejectionCommandHandler_Fail()
    {
        // Arrange
        var application = CreateValidApplicationForm();
        application.AcceptApplication(Money.FromPln(this.fixture.Create<decimal>()));
        application.RequestRecommendation(this.fixture.Create<DateTime>());
        application.EndorseApplication(application.Recommendations.First().Id);
        application.RegisterMembershipFeePayment(application.RequiredMembershipFee,this.fixture.Create<DateTime>());
        this.tenantSettingsService.GetTenantAppealPeriod(application.TenantId!.Value, Arg.Any<CancellationToken>())
            .Returns(TimeSpan.FromDays(14));
        var rejectionDate = this.fixture.Create<DateTime>();
        var appealDeadline = rejectionDate.AddDays(14);
        application.RejectApplication(rejectionDate,this.fixture.Create<string>(),appealDeadline);
        
        var command = this.fixture.Build<AppealApplicationRejectionCommand>()
            .With(x=>x.ApplicationId, application.AggregateId)
            .With(x=>x.TenantId, application.TenantId)
            .With(x=>x.AppealDate, rejectionDate.AddDays(17))
            .Create();
        
        var handler = new AppealApplicationRejectionCommandHandler(this.aggregateRepository);
        aggregateRepository.GetByIdAsync(application.AggregateId, application.TenantId, CancellationToken.None)
            .Returns(null as Domain.Model.ApplicationAggregate);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeTrue();
        application.Status.Should().Be(ApplicationStatus.Rejected);
        await aggregateRepository.Received(1).GetByIdAsync(application.AggregateId, application.TenantId, Arg.Any<CancellationToken>());
        await aggregateRepository.Received(0).SaveAsync(application, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task AppealApplicationRejectionCommandHandler_Handle_andAccept()
    {
        // Arrange
        var application = CreateValidApplicationForm();
        application.AcceptApplication(Money.FromPln(this.fixture.Create<decimal>()));
        application.RequestRecommendation(this.fixture.Create<DateTime>());
        application.EndorseApplication(application.Recommendations.First().Id);
        application.RegisterMembershipFeePayment(application.RequiredMembershipFee,this.fixture.Create<DateTime>());
        this.tenantSettingsService.GetTenantAppealPeriod(application.TenantId!.Value, Arg.Any<CancellationToken>())
            .Returns(TimeSpan.FromDays(14));
        var rejectionDate = this.fixture.Create<DateTime>();
        var appealDeadline = rejectionDate.AddDays(14);
        application.RejectApplication(rejectionDate,this.fixture.Create<string>(),appealDeadline);
        
        var command = this.fixture.Build<AppealApplicationRejectionCommand>()
            .With(x=>x.ApplicationId, application.AggregateId)
            .With(x=>x.TenantId, application.TenantId)
            .With(x=>x.AppealDate, rejectionDate.AddDays(7))
            .Create();
        
        var handler = new AppealApplicationRejectionCommandHandler(this.aggregateRepository);
        aggregateRepository.GetByIdAsync(application.AggregateId, application.TenantId, CancellationToken.None)
            .Returns(application);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeFalse();
        application.Status.Should().Be(ApplicationStatus.RejectionAppealed);
        await aggregateRepository.Received(1).GetByIdAsync(application.AggregateId, application.TenantId, Arg.Any<CancellationToken>());
        await aggregateRepository.Received(1).SaveAsync(application, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task ApproveAppealApplicationRejectionAppealCommandHandler_Handle()
    {
        // Arrange
        var application = CreateValidApplicationForm();
        application.AcceptApplication(Money.FromPln(this.fixture.Create<decimal>()));
        application.RequestRecommendation(this.fixture.Create<DateTime>());
        application.EndorseApplication(application.Recommendations.First().Id);
        application.RegisterMembershipFeePayment(application.RequiredMembershipFee,this.fixture.Create<DateTime>());
        this.tenantSettingsService.GetTenantAppealPeriod(application.TenantId!.Value, Arg.Any<CancellationToken>())
            .Returns(TimeSpan.FromDays(14));
        var rejectionDate = this.fixture.Create<DateTime>();
        var appealDeadline = rejectionDate.AddDays(14);
        application.RejectApplication(rejectionDate,this.fixture.Create<string>(),appealDeadline);
        application.AppealRejection(rejectionDate.AddDays(7), this.fixture.Create<string>());
        
        var command = this.fixture.Build<ApproveApplicationRejectionAppealCommand>()
            .With(x=>x.ApplicationId, application.AggregateId)
            .With(x=>x.TenantId, application.TenantId)
            .Create();
        
        var handler = new ApproveApplicationRejectionAppealCommandHandler(this.aggregateRepository);
        aggregateRepository.GetByIdAsync(application.AggregateId, application.TenantId, CancellationToken.None)
            .Returns(application);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeFalse();
        application.Status.Should().Be(ApplicationStatus.AppealApproved);
        await aggregateRepository.Received(1).GetByIdAsync(application.AggregateId, application.TenantId, Arg.Any<CancellationToken>());
        await aggregateRepository.Received(1).SaveAsync(application, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task ApproveAppealApplicationRejectionAppealCommandHandler_Fail()
    {
        // Arrange
        var application = CreateValidApplicationForm();
        application.AcceptApplication(Money.FromPln(this.fixture.Create<decimal>()));
        application.RequestRecommendation(this.fixture.Create<DateTime>());
        application.EndorseApplication(application.Recommendations.First().Id);
        application.RegisterMembershipFeePayment(application.RequiredMembershipFee,this.fixture.Create<DateTime>());
        this.tenantSettingsService.GetTenantAppealPeriod(application.TenantId!.Value, Arg.Any<CancellationToken>())
            .Returns(TimeSpan.FromDays(14));
        var rejectionDate = this.fixture.Create<DateTime>();
        var appealDeadline = rejectionDate.AddDays(14);
        application.RejectApplication(rejectionDate,this.fixture.Create<string>(),appealDeadline);
        application.AppealRejection(rejectionDate.AddDays(7), this.fixture.Create<string>());
        
        var command = this.fixture.Build<ApproveApplicationRejectionAppealCommand>()
            .With(x=>x.ApplicationId, application.AggregateId)
            .With(x=>x.TenantId, application.TenantId)
            .Create();
        
        var handler = new ApproveApplicationRejectionAppealCommandHandler(this.aggregateRepository);
        aggregateRepository.GetByIdAsync(application.AggregateId, application.TenantId, CancellationToken.None)
            .Returns(null as Domain.Model.ApplicationAggregate);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeTrue();
        application.Status.Should().Be(ApplicationStatus.RejectionAppealed);
        await aggregateRepository.Received(1).GetByIdAsync(application.AggregateId, application.TenantId, Arg.Any<CancellationToken>());
        await aggregateRepository.Received(0).SaveAsync(application, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task RejectAppealApplicationRejectionAppealCommandHandler_Handle()
    {
        // Arrange
        var application = CreateValidApplicationForm();
        application.AcceptApplication(Money.FromPln(this.fixture.Create<decimal>()));
        application.RequestRecommendation(this.fixture.Create<DateTime>());
        application.EndorseApplication(application.Recommendations.First().Id);
        application.RegisterMembershipFeePayment(application.RequiredMembershipFee,this.fixture.Create<DateTime>());
        this.tenantSettingsService.GetTenantAppealPeriod(application.TenantId!.Value, Arg.Any<CancellationToken>())
            .Returns(TimeSpan.FromDays(14));
        var rejectionDate = this.fixture.Create<DateTime>();
        var appealDeadline = rejectionDate.AddDays(14);
        application.RejectApplication(rejectionDate,this.fixture.Create<string>(),appealDeadline);
        application.AppealRejection(rejectionDate.AddDays(7), this.fixture.Create<string>());
        
        var command = this.fixture.Build<RejectApplicationRejectionAppealCommand>()
            .With(x=>x.ApplicationId, application.AggregateId)
            .With(x=>x.TenantId, application.TenantId)
            .Create();
        
        var handler = new RejectApplicationRejectionAppealCommandHandler(this.aggregateRepository);
        aggregateRepository.GetByIdAsync(application.AggregateId, application.TenantId, CancellationToken.None)
            .Returns(application);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeFalse();
        application.Status.Should().Be(ApplicationStatus.AppealRejected);
        await aggregateRepository.Received(1).GetByIdAsync(application.AggregateId, application.TenantId, Arg.Any<CancellationToken>());
        await aggregateRepository.Received(1).SaveAsync(application, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task RejectAppealApplicationRejectionAppealCommandHandler_Fail()
    {
        // Arrange
        var application = CreateValidApplicationForm();
        application.AcceptApplication(Money.FromPln(this.fixture.Create<decimal>()));
        application.RequestRecommendation(this.fixture.Create<DateTime>());
        application.EndorseApplication(application.Recommendations.First().Id);
        application.RegisterMembershipFeePayment(application.RequiredMembershipFee,this.fixture.Create<DateTime>());
        this.tenantSettingsService.GetTenantAppealPeriod(application.TenantId!.Value, Arg.Any<CancellationToken>())
            .Returns(TimeSpan.FromDays(14));
        var rejectionDate = this.fixture.Create<DateTime>();
        var appealDeadline = rejectionDate.AddDays(14);
        application.RejectApplication(rejectionDate,this.fixture.Create<string>(),appealDeadline);
        application.AppealRejection(rejectionDate.AddDays(7), this.fixture.Create<string>());
        
        var command = this.fixture.Build<RejectApplicationRejectionAppealCommand>()
            .With(x=>x.ApplicationId, application.AggregateId)
            .With(x=>x.TenantId, application.TenantId)
            .Create();
        
        var handler = new RejectApplicationRejectionAppealCommandHandler(this.aggregateRepository);
        aggregateRepository.GetByIdAsync(application.AggregateId, application.TenantId, CancellationToken.None)
            .Returns(null as Domain.Model.ApplicationAggregate);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeTrue();
        application.Status.Should().Be(ApplicationStatus.RejectionAppealed);
        await aggregateRepository.Received(1).GetByIdAsync(application.AggregateId, application.TenantId, Arg.Any<CancellationToken>());
        await aggregateRepository.Received(0).SaveAsync(application, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task CancelApplicationCommandHandler_Handle()
    {
        // Arrange
        var application = CreateValidApplicationForm();
        application.AcceptApplication(Money.FromPln(this.fixture.Create<decimal>()));
        application.RequestRecommendation(this.fixture.Create<DateTime>());
        var command = this.fixture.Build<CancelApplicationCommand>()
            .With(x=>x.ApplicationId, application.AggregateId)
            .With(x=>x.TenantId, application.TenantId)
            .Create();
        var handler = new CancelApplicationCommandHandler(this.aggregateRepository);
        aggregateRepository.GetByIdAsync(application.AggregateId, application.TenantId, CancellationToken.None)
            .Returns(application);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeFalse();
        application.Status.Should().Be(ApplicationStatus.Cancelled);
        await aggregateRepository.Received(1).GetByIdAsync(application.AggregateId, application.TenantId, Arg.Any<CancellationToken>());
        await aggregateRepository.Received(1).SaveAsync(application, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task CancelApplicationCommandHandler_Fail()
    {
        // Arrange
        var application = CreateValidApplicationForm();
        application.AcceptApplication(Money.FromPln(this.fixture.Create<decimal>()));
        application.RequestRecommendation(this.fixture.Create<DateTime>());
        var command = this.fixture.Build<CancelApplicationCommand>()
            .With(x=>x.ApplicationId, application.AggregateId)
            .With(x=>x.TenantId, application.TenantId)
            .Create();
        var handler = new CancelApplicationCommandHandler(this.aggregateRepository);
        aggregateRepository.GetByIdAsync(application.AggregateId, application.TenantId, CancellationToken.None)
            .Returns(null as Domain.Model.ApplicationAggregate);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsFaulted.Should().BeTrue();
        application.Status.Should().Be(ApplicationStatus.InRecommendation);
        await aggregateRepository.Received(1).GetByIdAsync(application.AggregateId, application.TenantId, Arg.Any<CancellationToken>());
        await aggregateRepository.Received(0).SaveAsync(application, Arg.Any<CancellationToken>());
    }
    
    private Domain.Model.ApplicationAggregate CreateValidApplicationForm()
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
        
        var applicationForm=  new Domain.Model.ApplicationAggregate(firstName,
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