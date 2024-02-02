namespace PLUG.ONPA.Common.Application.Dtos;

public abstract class Link
{
    public string Rel { get; private set; }
    public string Href { get; private set; }
    public string? Title { get; private set; }

    public Link(string relation, string href, string? title = null)
    {
        this.Rel = relation;
        this.Href = href;
        this.Title = title;
    }
}