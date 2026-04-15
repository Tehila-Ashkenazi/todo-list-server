using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace TodoApi;

public partial class ToDoDbContext : DbContext
{
    public ToDoDbContext()
    {
    }

    public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Item> Items { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    // => optionsBuilder.UseMySql("name=ToDoDB", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.44-mysql"));
    //       => optionsBuilder.UseMySql("Server=localhost;Database=tododb;User=root;Password=5806097;", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.44-mysql"));
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // נסיון למשוך את מחרוזת החיבור מהגדרות המערכת (Render)
        var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");
        // נסיון 2: אם הראשון נכשל, ננסה דרך הפורמט הסטנדרטי של דוט-נט
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
        }

        // אם אנחנו במחשב האישי (ולא ב-Render), נשתמש בברירת המחדל המקומית
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = "Server=localhost;Database=tododb;User=root;Password=5806097;";
        }

        optionsBuilder.UseMySql(connectionString,
            ServerVersion.AutoDetect(connectionString),
            options => options.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("items");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
