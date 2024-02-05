using PLUG.ONPA.Shared.ExternalCommunication.Models;

namespace PLUG.ONPA.Shared.ExternalCommunication.Abstractions;

public interface IEmailService
{
    Task<Guid> SendEmailAsync(Email email, CancellationToken cancellationToken = default);
    
    
}