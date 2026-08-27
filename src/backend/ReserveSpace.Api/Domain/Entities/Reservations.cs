namespace ReserveSpace.Api.Domain.Entities;

public class Reservations
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    
    // --- Час ---
    public DateTime StartTime { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // --- Prices --- 
    public decimal HallPrice { get; set; }
    public decimal AdditionsPrice { get; set; }
    public decimal TotalPrice { get; set; }
    
    // --- Navigations --- 
    public Rooms Room { get; set; } = null!;
    public ICollection<ReservationAdditions> ReservationAdditions { get; set; } = [];
}