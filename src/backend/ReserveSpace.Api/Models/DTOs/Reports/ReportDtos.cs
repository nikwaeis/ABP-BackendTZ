namespace ReserveSpace.Api.Models.DTOs.Reports;

/// <summary>
/// DTO загального аналітичного звіту для бізнесу.
/// </summary>
public class BusinessAnalyticsReportDto
{
    /// <summary>
    /// Загальна виручка за обраний період (грн).
    /// </summary>
    public decimal TotalRevenue { get; set; }

    /// <summary>
    /// Виручка тільки від оренди залів (грн).
    /// </summary>
    public decimal HallRevenue { get; set; }

    /// <summary>
    /// Виручка від додаткових послуг (грн).
    /// </summary>
    public decimal AdditionsRevenue { get; set; }

    /// <summary>
    /// Загальна кількість бронювань.
    /// </summary>
    public int TotalReservationsCount { get; set; }

    /// <summary>
    /// Загальна кількість заброньованих годин.
    /// </summary>
    public double TotalBookedHours { get; set; }

    /// <summary>
    /// Середній чек за бронювання (грн).
    /// </summary>
    public decimal AverageCheck { get; set; }

    /// <summary>
    /// Статистика залів за популярністю та доходом.
    /// </summary>
    public List<RoomPopularityReportDto> RoomStats { get; set; } = [];

    /// <summary>
    /// Статистика популярності додаткових послуг.
    /// </summary>
    public List<AdditionPopularityReportDto> AdditionStats { get; set; } = [];
}

/// <summary>
/// Статистика використання конкретного залу.
/// </summary>
public class RoomPopularityReportDto
{
    public Guid RoomId { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public int ReservationsCount { get; set; }
    public double TotalHoursBooked { get; set; }
    public decimal TotalRevenue { get; set; }
}

/// <summary>
/// Статистика замовлення конкретної послуги.
/// </summary>
public class AdditionPopularityReportDto
{
    public Guid AdditionId { get; set; }
    public string AdditionName { get; set; } = string.Empty;
    public int OrderCount { get; set; }
    public decimal TotalRevenue { get; set; }
}
