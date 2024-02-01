using LanguageExt.Common;
using MediatR;
using PLUG.ONPA.Common.Application.Abstractions;

namespace PLUG.ONPA.Common.Application;

public abstract record CommandBase(Guid? TenantId) : ICommand, IRequest<Result<bool>>;
