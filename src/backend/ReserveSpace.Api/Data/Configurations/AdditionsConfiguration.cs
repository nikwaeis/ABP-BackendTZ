using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReserveSpace.Api.Domain.Entities;

namespace ReserveSpace.Api.Data.Configurations;

public class AdditionsConfiguration : IEntityTypeConfiguration<Additions>
{
    public void Configure(EntityTypeBuilder<Additions> builder)
    {
        builder.ToTable("additions");
        
        builder.HasKey(additions => additions.Id);
        
        builder
            .Property(additions => additions.Name)
            .HasMaxLength(30)
            .IsRequired();
        
        builder
            .Property(additions => additions.Price)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder
            .Property(additions => additions.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        builder
            .Property(additions => additions.UpdatedAt)
            .HasColumnType("timestamp with time zone")
            .IsRequired(false);
        
        builder
            .HasOne(additions => additions.Room)
            .WithMany(room => room.Additions)
            .HasForeignKey(additions => additions.RoomId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
            
    }
}