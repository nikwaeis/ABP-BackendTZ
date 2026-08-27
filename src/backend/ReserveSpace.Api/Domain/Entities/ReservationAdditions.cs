namespace ReserveSpace.Api.Domain.Entities;

/// <summary>
/// Представляє зв'язок між бронюванням та обраною додатковою послугою.
/// </summary>
public class ReservationAdditions
{
    /// <summary>
    /// Унікальний ідентифікатор запису.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Ідентифікатор бронювання.
    /// </summary>
    public Guid ReservationId { get; set; }

    /// <summary>
    /// Ідентифікатор додаткової послуги.
    /// </summary>
    public Guid AdditionId { get; set; }

    #region Navigation Properties

    /// <summary>
    /// Обрана додаткова послуга.
    /// </summary>
    public Additions Addition { get; set; } = null!;

    /// <summary>
    /// Бронювання, до якого додано послугу.
    /// </summary>
    public Reservations Reservation { get; set; } = null!;

    #endregion
}