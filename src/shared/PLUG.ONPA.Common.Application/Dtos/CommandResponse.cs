namespace PLUG.ONPA.Common.Application.Dtos;

public abstract class CommandResponse
{
    private readonly List<Link> links = new List<Link>();
    public IEnumerable<Link> Links => this.links;

    public void AddLink(Link link)
    {
        this.links.Add(link);
    }
    public void AddLinks(params Link[] links)
    {
        links.ToList().ForEach(l => this.links.Add(l));
    }
}