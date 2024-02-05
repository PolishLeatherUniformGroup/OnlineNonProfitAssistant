using PLUG.ONPA.Common.Application.Abstractions;

namespace PLUG.ONPA.Common.Application;

public abstract record QueryBase<TResult> : IQuery<TResult>
{
    
}