namespace ReserveSpace.Api.Domain.Entities;

/// <summary>
/// Представляє бронювання приміщення/залу на певний проміжок часу.
/// </summary>
public class Reservations
{
    /// <summary>
    /// Унікальний ідентифікатор бронювання.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Ідентифікатор заброньованого залу.
    /// </summary>
    public Guid RoomId { get; set; }
    
    // --- Час ---

    /// <summary>
    /// Дата та час початку бронювання (UTC).
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Тривалість бронювання.
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Дата та час завершення бронювання (UTC).
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Дата та час створення запису про бронювання (UTC).
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    // --- Ціни (Prices) --- 

    /// <summary>
    /// Вартість оренди самого залу з урахуванням погодинних тарифів і коефіцієнтів.
    /// </summary>
    public decimal HallPrice { get; set; }

    /// <summary>
    /// Загальна вартість обраних додаткових послуг.
    /// </summary>
    public decimal AdditionsPrice { get; set; }

    /// <summary>
    /// Загальна підсумкова вартість бронювання (зал + послуги).
    /// </summary>
    public decimal TotalPrice { get; set; }
    
    // --- Navigations --- 

    /// <summary>
    /// Заброньований зал.
    /// </summary>
    public Rooms Room { get; set; } = null!;

    /// <summary>
    /// Список обраних додаткових послуг для цього бронювання.
    /// </summary>
    public ICollection<ReservationAdditions> ReservationAdditions { get; set; } = [];
}