using Microsoft.EntityFrameworkCore;
using YaTrackerParser.Data.Context.Entites;

namespace YaTrackerParser.Data.Context;

/// <summary>
/// Класс для подключения к БД
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Создание таблицы Tickets
    /// </summary>
    public DbSet<TicketEntity> Tickets { get; set; }

    /// <summary>
    /// Подключение к БД
    /// </summary>
    /// <param name="options"></param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    /// <summary>
    /// Создание модели в БД
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TicketEntity>().HasKey(t => t.Id);
    }
}
