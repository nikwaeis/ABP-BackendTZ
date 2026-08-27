using ReserveSpace.Api.Models.DTOs.Reports;
using ReserveSpace.Api.Models.DTOs.Reservations;

namespace ReserveSpace.Api.Services.Abstractions;

/// <summary>
/// Сервіс для управління бронюваннями та розрахунку вартості оренди.
/// </summary>
public interface IReservationService
{
    /// <summary>
    /// Отримати детальну інформацію про бронювання за ID.
    /// </summary>
    Task<ReservationResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Створити нове бронювання з розрахунком вартості.
    /// </summary>
    Task<ReservationResponseDto> CreateReservationAsync(CreateReservationRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Отримати аналітичний звіт для бізнесу за вказаний період.
    /// </summary>
    Task<BusinessAnalyticsReportDto> GetAnalyticsReportAsync(DateTime? fromUtc, DateTime? toUtc, CancellationToken cancellationToken = default);
}
