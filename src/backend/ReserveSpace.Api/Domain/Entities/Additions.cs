namespace ReserveSpace.Api.Domain.Entities;

public class Additions
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Rooms Room { get; set; } = null!;
}