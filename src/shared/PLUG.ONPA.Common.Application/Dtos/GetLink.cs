namespace PLUG.ONPA.Common.Application.Dtos;

public sealed class GetLink : Link
{
    public GetLink(string href, string? title = null) : base("GET", href, title)
    {
    }
}