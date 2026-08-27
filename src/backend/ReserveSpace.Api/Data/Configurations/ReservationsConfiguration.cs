using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReserveSpace.Api.Domain.Entities;

namespace ReserveSpace.Api.Data.Configurations;

public class ReservationsConfiguration : IEntityTypeConfiguration<Reservations>
{
    public void Configure(EntityTypeBuilder<Reservations> builder)
    {
        builder.ToTable("reservations");
        
        builder.HasKey(reservations => reservations.Id);
        
        builder.HasIndex(reservations => new {reservations.RoomId, reservations.StartTime, reservations.EndTime});

        builder
            .Property(reservations => reservations.StartTime)
            .HasColumnType("timestamp with time zone")
            .IsRequired();
        
        builder
            .Property(reservations => reservations.Duration)
            .IsRequired();

        builder
            .Property(reservations => reservations.EndTime)
            .HasColumnType("timestamp with time zone")
            .IsRequired();
        
        builder
            .Property(reservations => reservations.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        builder
            .Property(reservations => reservations.HallPrice)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder
            .Property(reservations => reservations.AdditionsPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder
            .Property(reservations => reservations.TotalPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder
            .HasOne(reservations => reservations.Room)
            .WithMany(room => room.Reservations)
            .HasForeignKey(reservations => reservations.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}