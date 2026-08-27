using System.ComponentModel.DataAnnotations;
using ReserveSpace.Api.Models.DTOs.Additions;

namespace ReserveSpace.Api.Models.DTOs.Rooms;

/// <summary>
/// DTO запиту на створення конференц-залу.
/// </summary>
public class CreateRoomRequestDto
{
    /// <summary>
    /// Назва залу (наприклад, "Зал А").
    /// </summary>
    [Required(ErrorMessage = "Назва залу є обов'язковою")]
    [MaxLength(30, ErrorMessage = "Максимальна довжина назви залу - 30 символів")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Місткість залу (кількість осіб).
    /// </summary>
    [Range(1, 10000, ErrorMessage = "Місткість залу повинна бути від 1 особи")]
    public int Capacity { get; set; }

    /// <summary>
    /// Базова вартість оренди за одну годину (в гривнях).
    /// </summary>
    [Range(0.01, 1000000, ErrorMessage = "Базова вартість повинна бути більше 0")]
    public decimal PricePerHour { get; set; }

    /// <summary>
    /// Список доступних послуг для цього залу.
    /// </summary>
    public List<AdditionRequestDto>? Additions { get; set; } = [];
}

/// <summary>
/// DTO запиту на оновлення даних конференц-залу.
/// </summary>
public class UpdateRoomRequestDto
{
    /// <summary>
    /// Назва залу.
    /// </summary>
    [Required(ErrorMessage = "Назва залу є обов'язковою")]
    [MaxLength(30, ErrorMessage = "Максимальна довжина назви залу - 30 символів")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Місткість залу.
    /// </summary>
    [Range(1, 10000, ErrorMessage = "Місткість залу повинна бути від 1 особи")]
    public int Capacity { get; set; }

    /// <summary>
    /// Базова вартість оренди за годину.
    /// </summary>
    [Range(0.01, 1000000, ErrorMessage = "Базова вартість повинна бути більше 0")]
    public decimal PricePerHour { get; set; }

    /// <summary>
    /// Оновлений повний список послуг для цього залу.
    /// </summary>
    public List<AdditionRequestDto>? Additions { get; set; }
}

/// <summary>
/// DTO запиту на пошук доступних залів.
/// </summary>
public class SearchAvailableRoomsRequestDto
{
    /// <summary>
    /// Дата та час початку оренди (UTC).
    /// </summary>
    [Required(ErrorMessage = "Час початку є обов'язковим")]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Дата та час завершення оренди (UTC).
    /// </summary>
    [Required(ErrorMessage = "Час завершення є обов'язковим")]
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Необхідна мінімальна місткість (кількість осіб).
    /// </summary>
    [Range(1, 10000, ErrorMessage = "Місткість повинна бути більшою за 0")]
    public int Capacity { get; set; }
}

/// <summary>
/// DTO відповіді з інформацією про конференц-зал.
/// </summary>
public class RoomResponseDto
{
    /// <summary>
    /// Унікальний ідентифікатор залу.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Назва залу.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Місткість (осіб).
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// Базова вартість оренди за годину.
    /// </summary>
    public decimal PricePerHour { get; set; }

    /// <summary>
    /// Список доступних додаткових послуг.
    /// </summary>
    public List<AdditionResponseDto> Additions { get; set; } = [];

    /// <summary>
    /// Дата створення запису.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата останнього оновлення.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
