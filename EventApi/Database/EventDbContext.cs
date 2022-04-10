using Event.Model;
using Microsoft.EntityFrameworkCore;

namespace Event.Database;

public class EventDbContext : DbContext
{
    public DbSet<Organisation> Organisations { get; set; } = null!;
    public DbSet<Model.Event> Events { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Team> Teams { get; set; } = null!;

    public EventDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("Event");

        builder.Entity<Organisation>(org =>
        {
            org.HasMany(o => o.Events)
                .WithOne(e => e.Organisation)
                .HasForeignKey(e => e.OrganisationId);
            org.HasMany(o => o.Members);
        });

        builder.Entity<Model.Event>(evt =>
        {
            evt.HasMany(e => e.Teams)
                .WithOne(t => t.Event)
                .HasForeignKey(t => t.EventId);
        });
    }
}