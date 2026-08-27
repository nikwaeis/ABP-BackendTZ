using System.ComponentModel.DataAnnotations;
using ReserveSpace.Api.Models.DTOs.Additions;

namespace ReserveSpace.Api.Models.DTOs.Reservations;

/// <summary>
/// DTO запиту на створення бронювання.
/// </summary>
public class CreateReservationRequestDto
{
    /// <summary>
    /// Ідентифікатор залу для бронювання.
    /// </summary>
    [Required(ErrorMessage = "ID залу є обов'язковим")]
    public Guid RoomId { get; set; }

    /// <summary>
    /// Дата та час початку бронювання (UTC).
    /// </summary>
    [Required(ErrorMessage = "Дата і час початку бронювання є обов'язковими")]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Тривалість бронювання (наприклад, "02:00:00" або кількість годин).
    /// </summary>
    [Required(ErrorMessage = "Тривалість бронювання є обов'язковою")]
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Список ID обраних додаткових послуг.
    /// </summary>
    public List<Guid>? SelectedAdditionIds { get; set; } = [];
}

/// <summary>
/// DTO розбивки вартості оренди за годинами.
/// </summary>
public class HourlyBreakdownDto
{
    /// <summary>
    /// Початок годинного інтервалу.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Кінець годинного інтервалу.
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Опис тарифного періоду (Стандартний, Ранковий зі знижкою 10%, Піковий з націнкою 15%, Вечірній зі знижкою 20%).
    /// </summary>
    public string RateTypeDescription { get; set; } = string.Empty;

    /// <summary>
    /// Застосований коефіцієнт ціни (наприклад, 1.0, 0.9, 1.15, 0.8).
    /// </summary>
    public decimal RateMultiplier { get; set; }

    /// <summary>
    /// Розрахована вартість за цей інтервал.
    /// </summary>
    public decimal CalculatedPrice { get; set; }
}

/// <summary>
/// DTO підтвердження бронювання з детальним розрахунком вартості.
/// </summary>
public class ReservationResponseDto
{
    /// <summary>
    /// Унікальний ідентифікатор створеного бронювання.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Ідентифікатор залу.
    /// </summary>
    public Guid RoomId { get; set; }

    /// <summary>
    /// Назва заброньованого залу.
    /// </summary>
    public string RoomName { get; set; } = string.Empty;

    /// <summary>
    /// Дата та час початку оренди (UTC).
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Тривалість оренди.
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Дата та час завершення оренди (UTC).
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Вартість оренди залу (з урахуванням часових тарифів та знижок).
    /// </summary>
    public decimal HallPrice { get; set; }

    /// <summary>
    /// Загальна вартість обраних додаткових послуг.
    /// </summary>
    public decimal AdditionsPrice { get; set; }

    /// <summary>
    /// Загальна підсумкова вартість бронювання.
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Деталізація погодинного розрахунку оренди залу.
    /// </summary>
    public List<HourlyBreakdownDto> PriceBreakdown { get; set; } = [];

    /// <summary>
    /// Список обраних додаткових послуг.
    /// </summary>
    public List<AdditionResponseDto> SelectedAdditions { get; set; } = [];

    /// <summary>
    /// Дата створення бронювання.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
