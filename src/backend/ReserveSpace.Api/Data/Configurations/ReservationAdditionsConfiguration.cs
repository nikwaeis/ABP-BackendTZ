using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReserveSpace.Api.Domain.Entities;

namespace ReserveSpace.Api.Data.Configurations;

public class ReservationAdditionsConfiguration : IEntityTypeConfiguration<ReservationAdditions>
{
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