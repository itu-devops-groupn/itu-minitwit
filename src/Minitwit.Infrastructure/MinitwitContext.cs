namespace Minitwit.Infrastructure;

using Microsoft.EntityFrameworkCore;

public class MinitwitContext : DbContext
{
    public DbSet<Message> Messages { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Follower> Followers { get; set; }

    public MinitwitContext(DbContextOptions<MinitwitContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.User_id);
        modelBuilder.Entity<User>().Property(u => u.Username).IsRequired();
        modelBuilder.Entity<User>().Property(u => u.Email).IsRequired();
        modelBuilder.Entity<User>().Property(u => u.Pw_hash).IsRequired();

        modelBuilder.Entity<Follower>().HasKey(f => new { f.Who_id, f.Whom_id });

        modelBuilder.Entity<Message>().HasKey(m => m.Message_id);
        modelBuilder.Entity<Message>().Property(m => m.Message_id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Message>().Property(m => m.Author_id).IsRequired();
        modelBuilder.Entity<Message>().Property(m => m.Text).IsRequired();
    }
}
