namespace Chirp.Infrastructure;

using Microsoft.EntityFrameworkCore;

public class ChirpContext : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

    public ChirpContext(DbContextOptions<ChirpContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cheep>().Property(c => c.Text).HasMaxLength(160).IsRequired();
        modelBuilder.Entity<Cheep>().Property(c => c.TimeStamp).IsRequired();
        modelBuilder.Entity<Cheep>().Property(c => c.AuthorId).IsRequired();
        modelBuilder.Entity<Cheep>().Property(c => c.CheepId).IsRequired();
        modelBuilder.Entity<Cheep>().HasIndex(c => c.CheepId).IsUnique();

        modelBuilder.Entity<Author>().Property(a => a.AuthorId).IsRequired();
        modelBuilder.Entity<Author>().Property(a => a.Name).IsRequired();
        modelBuilder.Entity<Author>().Property(a => a.Email).IsRequired();
        modelBuilder.Entity<Author>().HasIndex(a => a.AuthorId).IsUnique();
        modelBuilder.Entity<Author>().HasIndex(a => a.Name).IsUnique();
        modelBuilder.Entity<Author>().HasIndex(a => a.Email).IsUnique();
    }
}
