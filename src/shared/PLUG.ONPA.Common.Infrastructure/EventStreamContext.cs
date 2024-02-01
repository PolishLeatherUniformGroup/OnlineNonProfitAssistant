using Microsoft.EntityFrameworkCore;
using PLUG.ONPA.Common.Infrastructure.Storage;

namespace PLUG.ONPA.Common.Infrastructure;

public class EventStreamContext : DbContext
{
    public DbSet<EventEntry> EventsStream { get; set; }
    
    public DbSet<EventStatus> EventsStatus { get; set; }
    
    public DbSet<DomainEventEntry> DomainEvents { get; set; }
}