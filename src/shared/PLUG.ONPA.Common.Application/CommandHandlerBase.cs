using LanguageExt.Common;
using MediatR;

namespace PLUG.ONPA.Common.Application;

public abstract class CommandHandlerBase<TCommand> : IRequestHandler<TCommand, Result<bool>>
    where TCommand : CommandBase
{
    public abstract Task<Result<bool>> Handle(TCommand request, CancellationToken cancellationToken);
}