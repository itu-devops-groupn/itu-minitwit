namespace Chirp.Infrastructure;

using Microsoft.EntityFrameworkCore;

public class ChirpContext : DbContext
{
    public DbSet<Message> Messages { get; set; }
    public DbSet<User> Users { get; set; }

    public ChirpContext(DbContextOptions<ChirpContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.user_id);
        modelBuilder.Entity<User>().Property(u => u.username).IsRequired();
        modelBuilder.Entity<User>().Property(u => u.email).IsRequired();
        modelBuilder.Entity<User>().Property(u => u.pw_hash).IsRequired();

        modelBuilder.Entity<Follower>().HasKey(f => new { f.who_id, f.whom_id });

        modelBuilder.Entity<Message>().HasKey(m => m.message_id).Property(m => m.message_id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Message>().Property(m => m.author_id).IsRequired();
        modelBuilder.Entity<Message>().Property(m => m.Text).IsRequired();
    }
}
