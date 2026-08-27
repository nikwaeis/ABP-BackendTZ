using Microsoft.EntityFrameworkCore;
using ReserveSpace.Api.Domain.Entities;

namespace ReserveSpace.Api.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options) 
{  
    
    // ---- Основні істоти ----
    public DbSet<Rooms> Rooms => Set<Rooms>();
    public DbSet<Additions>  Additions => Set<Additions>();
    
    // ---- Істоти резервації ---- 
    public DbSet<Reservations>  Reservations => Set<Reservations>();
    public DbSet<ReservationAdditions> ReservationAddtions => Set<ReservationAdditions>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}