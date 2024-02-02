namespace PLUG.ONPA.Common.Application.Dtos;

public abstract class SingleResponse<TData> where TData: notnull
{
    public TData Data { get; set; }
    
    protected SingleResponse(TData data)
    {
        this.Data = data;
    }
}