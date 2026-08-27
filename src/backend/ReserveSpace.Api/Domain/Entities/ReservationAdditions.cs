namespace ReserveSpace.Api.Domain.Entities;

public class ReservationAdditions
{
    public Guid Id { get; set; }
    public Guid ReservationId { get; set; }
    public Guid AdditionId { get; set; }
    public Additions Addition { get; set; } = null!;

    public Reservations Reservation { get; set; } = null!;
}