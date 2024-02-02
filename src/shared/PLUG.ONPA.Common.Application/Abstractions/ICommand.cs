using LanguageExt.Common;
using MediatR;

namespace PLUG.ONPA.Common.Application.Abstractions;

public interface ICommand : IRequest<Result<Guid>>;