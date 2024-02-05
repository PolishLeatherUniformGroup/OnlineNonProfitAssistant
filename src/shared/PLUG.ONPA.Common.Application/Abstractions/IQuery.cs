using LanguageExt.Common;
using MediatR;

namespace PLUG.ONPA.Common.Application.Abstractions;

public interface IQuery<TResult> : IRequest<Result<TResult>>
{
    
}