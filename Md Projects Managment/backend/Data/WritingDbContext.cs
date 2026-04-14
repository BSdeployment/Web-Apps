using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class WritingDbContext : DbContext
{
    public WritingDbContext(DbContextOptions<WritingDbContext> options) : base(options)
    {
    }

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<ProjectFile> ProjectFiles => Set<ProjectFile>();
    public DbSet<ProjectLink> ProjectLinks => Set<ProjectLink>();
    public DbSet<ProjectNote> ProjectNotes => Set<ProjectNote>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Article)
            .WithOne(a => a.Project)
            .HasForeignKey<Article>(a => a.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Project>()
            .HasOne(p => p.Note)
            .WithOne(n => n.Project)
            .HasForeignKey<ProjectNote>(n => n.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Project>()
            .HasMany(p => p.Files)
            .WithOne(f => f.Project)
            .HasForeignKey(f => f.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Project>()
            .HasMany(p => p.Links)
            .WithOne(l => l.Project)
            .HasForeignKey(l => l.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Article>()
            .HasIndex(a => a.ProjectId)
            .IsUnique();

        modelBuilder.Entity<ProjectNote>()
            .HasIndex(n => n.ProjectId)
            .IsUnique();
    }
}
