namespace ReserveSpace.Api.Domain.Entities;

/// <summary>
/// Представляє додаткову послугу або обладнання (наприклад, Проєктор, Wi-Fi, Звук).
/// </summary>
public class Additions
{
    /// <summary>
    /// Унікальний ідентифікатор додаткової послуги.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Ідентифікатор приміщення/залу, до якого прив'язана послуга.
    /// </summary>
    public Guid RoomId { get; set; }

    /// <summary>
    /// Назва додаткової послуги.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Вартість послуги.
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Дата та час створення запису (UTC).
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата та час останнього оновлення запису (UTC).
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    #region Navigation Properties

    /// <summary>
    /// Приміщення, до якого належить ця послуга.
    /// </summary>
    public Rooms Room { get; set; } = null!;

    #endregion
}