using LanguageExt.Common;
using MediatR;

namespace PLUG.ONPA.Common.Application;

public abstract class CommandHandlerBase<TCommand> : IRequestHandler<TCommand, Result<Guid>>
    where TCommand : CommandBase
{
    public abstract Task<Result<Guid>> Handle(TCommand request, CancellationToken cancellationToken);
}