namespace ReserveSpace.Api.Domain.Entities;

/// <summary>
/// Представляє приміщення/кімнату, доступну для бронювання.
/// </summary>
public class Rooms
{
    /// <summary>
    /// Унікальний ідентифікатор кімнати.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Назва або номер кімнати (наприклад, "Конференц-зал A").
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Максимальна місткість (кількість осіб).
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// Вартість оренди за одну годину.
    /// </summary>
    public decimal PricePerHour { get; set; }

    /// <summary>
    /// Дата та час створення запису (UTC).
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Дата та час останнього оновлення запису (UTC).
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    #region Navigation Properties (Навігаційні властивості)

    /// <summary>
    /// Список бронювань для цієї кімнати.
    /// </summary>
    public ICollection<Reservations> Reservations { get; set; } = [];
    public ICollection<Additions> Additions { get; set; } = [];

    #endregion
}
