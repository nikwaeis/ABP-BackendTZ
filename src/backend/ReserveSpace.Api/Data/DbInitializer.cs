using Microsoft.EntityFrameworkCore;
using ReserveSpace.Api.Data;
using ReserveSpace.Api.Domain.Entities;

namespace ReserveSpace.Api.Data;

/// <summary>
/// Ініціалізатор початкових даних (Seed Data) згідно з ТЗ:
/// Зали:
/// - Зал A: місткість 50 осіб, базова вартість 2000 грн/год.
/// - Зал B: місткість 100 осіб, базова вартість 3500 грн/год.
/// - Зал C: місткість 30 осіб, базова вартість 1500 грн/год.
/// Послуги:
/// - Проєктор: 500 грн.
/// - Wi-Fi: 300 грн.
/// - Звук: 700 грн.
/// </summary>
public static class DbInitializer
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Rooms.AnyAsync())
        {
            return; // Дані вже є
        }

        var roomA = new Rooms
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "Зал A",
            Capacity = 50,
            PricePerHour = 2000m,
            CreatedAt = DateTime.UtcNow,
            Additions =
            [
                new Additions { Id = Guid.NewGuid(), Name = "Проєктор", Price = 500m, CreatedAt = DateTime.UtcNow },
                new Additions { Id = Guid.NewGuid(), Name = "Wi-Fi", Price = 300m, CreatedAt = DateTime.UtcNow },
                new Additions { Id = Guid.NewGuid(), Name = "Звук", Price = 700m, CreatedAt = DateTime.UtcNow }
            ]
        };

        var roomB = new Rooms
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Name = "Зал B",
            Capacity = 100,
            PricePerHour = 3500m,
            CreatedAt = DateTime.UtcNow,
            Additions =
            [
                new Additions { Id = Guid.NewGuid(), Name = "Проєктор", Price = 500m, CreatedAt = DateTime.UtcNow },
                new Additions { Id = Guid.NewGuid(), Name = "Wi-Fi", Price = 300m, CreatedAt = DateTime.UtcNow },
                new Additions { Id = Guid.NewGuid(), Name = "Звук", Price = 700m, CreatedAt = DateTime.UtcNow }
            ]
        };

        var roomC = new Rooms
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Name = "Зал C",
            Capacity = 30,
            PricePerHour = 1500m,
            CreatedAt = DateTime.UtcNow,
            Additions =
            [
                new Additions { Id = Guid.NewGuid(), Name = "Проєктор", Price = 500m, CreatedAt = DateTime.UtcNow },
                new Additions { Id = Guid.NewGuid(), Name = "Wi-Fi", Price = 300m, CreatedAt = DateTime.UtcNow },
                new Additions { Id = Guid.NewGuid(), Name = "Звук", Price = 700m, CreatedAt = DateTime.UtcNow }
            ]
        };

        context.Rooms.AddRange(roomA, roomB, roomC);
        await context.SaveChangesAsync();
    }
}
