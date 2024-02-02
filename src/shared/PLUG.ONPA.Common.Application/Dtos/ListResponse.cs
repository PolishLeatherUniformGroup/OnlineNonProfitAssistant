namespace PLUG.ONPA.Common.Application.Dtos;

public abstract class ListResponse<TData> where TData:  notnull
{
    public List<TData> Data { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    
    protected ListResponse(ICollection<TData> data, int totalCount=0, int pageSize=10, int pageNumber=1)
    {
        this.Data = data.ToList();
        this.TotalCount = totalCount;
        this.PageSize = pageSize;
        this.PageNumber = pageNumber;
    }
}