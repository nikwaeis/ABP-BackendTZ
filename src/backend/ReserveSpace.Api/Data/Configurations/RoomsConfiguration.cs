using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReserveSpace.Api.Domain.Entities;

namespace ReserveSpace.Api.Data.Configurations;

/// <summary>
/// Конфігурація сутності <see cref="Rooms"/> для Entity Framework Core.
/// </summary>
public class RoomsConfiguration : IEntityTypeConfiguration<Rooms>
{
    /// <summary>
    /// Налаштовує схему таблиці та обмеження для сутності конференц-залів.
    /// </summary>
    /// <param name="builder">Будівник для конфігурації сутності.</param>
    public void Configure(EntityTypeBuilder<Rooms> builder)
    {
        builder.ToTable("rooms");
        
        builder.HasKey(room => room.Id);
        
        builder
            .Property(room => room.Name)
            .HasMaxLength(30)
            .IsRequired();
        
        builder
            .Property(room => room.PricePerHour)
            .HasPrecision(18,2)
            .IsRequired();

        builder
            .Property(room => room.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
            
        builder
            .Property(x => x.UpdatedAt)
            .HasColumnType("timestamp with time zone")
            .IsRequired(false);
    }
}