using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReserveSpace.Api.Domain.Entities;

namespace ReserveSpace.Api.Data.Configurations;

/// <summary>
/// Конфігурація сутності <see cref="ReservationAdditions"/> для Entity Framework Core.
/// </summary>
public class ReservationAdditionsConfiguration : IEntityTypeConfiguration<ReservationAdditions>
{
    /// <summary>
    /// Налаштовує схему таблиці та зв'язки для обраних послуг у бронюванні.
    /// </summary>
    /// <param name="builder">Будівник для конфігурації сутності.</param>
    public void Configure(EntityTypeBuilder<ReservationAdditions> builder)
    {
        builder.ToTable("reservations_additions");
        
        builder.HasKey(reservationAdditions => reservationAdditions.Id);
        
        builder
            .HasOne(ra => ra.Addition)
            .WithMany()
            .HasForeignKey(ra => ra.AdditionId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(reservationAdditions => reservationAdditions.Reservation)
            .WithMany(reservation => reservation.ReservationAdditions)
            .HasForeignKey(reservationAdditions => reservationAdditions.ReservationId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}