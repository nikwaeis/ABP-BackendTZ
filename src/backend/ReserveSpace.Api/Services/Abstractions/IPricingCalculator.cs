using ReserveSpace.Api.Models.DTOs.Reservations;

namespace ReserveSpace.Api.Services.Abstractions;

/// <summary>
/// Сервіс для розрахунку вартості оренди приміщень за часовими інтервалами та тарифами.
/// </summary>
public interface IPricingCalculator
{
    /// <summary>
    /// Розраховує погодинну вартість оренди залу за вказаний період.
    /// </summary>
    /// <param name="basePricePerHour">Базова погодинна ставка залу.</param>
    /// <param name="start">Початок бронювання (UTC/локальний).</param>
    /// <param name="duration">Тривалість бронювання.</param>
    /// <returns>Загальна вартість оренди залу та розбивка за інтервалами.</returns>
    (decimal TotalHallPrice, List<HourlyBreakdownDto> Breakdown) CalculateHallPrice(
        decimal basePricePerHour, 
        DateTime start, 
        TimeSpan duration);
}
