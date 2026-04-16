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
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    // טעינת המשתנה מה-Environment של Render
    //    var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");

    //    if (!string.IsNullOrEmpty(connectionString))
    //    {
    //        // שימוש במחרוזת מהענן
    //        optionsBuilder.UseMySql(connectionString,
    //            ServerVersion.AutoDetect(connectionString),
    //            options => options.EnableRetryOnFailure(
    //                maxRetryCount: 10,
    //                maxRetryDelay: TimeSpan.FromSeconds(30),
    //                errorNumbersToAdd: null));
    //    }
    //    else
    //    {
    //        // ברירת מחדל למחשב בבית - ודאי שזה localhost אצלך
    //        var localConnectionString = "Server=localhost;Database=tododb;Uid=root;Pwd=5806097;";
    //        optionsBuilder.UseMySql(localConnectionString,
    //            ServerVersion.AutoDetect(localConnectionString));
    //    }
    //}
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // מבחן אמת: אנחנו לא שואלים את Render כלום. אנחנו מכריחים את השרת ללכת ישירות לענן!
        //string hardcodedConnectionString = "Server=bjzwz3fhld62wsjqvx4e-mysql.services.clever-cloud.com;Database=bjzwz3fhld62wsjqvx4e;Uid=umfwn9me3yidnf83;Pwd=EOJFGdAtwNwjuXzXOkT1;SslMode=None;";
        string hardcodedConnectionString =
            "Server=bjzwz3fhld62wsjqvx4e-mysql.services.clever-cloud.com;Database=bjzwz3fhld62wsjqvx4e;Uid=umfwn9me3yidnf83;Pwd=EOJFGdAtwNwjuXzXOkT1;SslMode=Required;";

        optionsBuilder.UseMySql(hardcodedConnectionString,
            ServerVersion.AutoDetect(hardcodedConnectionString),
            options => options.EnableRetryOnFailure());
    }
  
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Items");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
