using ReserveSpace.Api.Models.DTOs.Reservations;
using ReserveSpace.Api.Services.Abstractions;

namespace ReserveSpace.Api.Services.Implementations;

/// <summary>
/// Реалізація розрахунку вартості оренди відповідно до тарифних періодів:
/// - 06:00 - 09:00: Знижка 10% (Ранкові години)
/// - 09:00 - 18:00: Базова ставка (Стандартні години)
/// - 12:00 - 14:00: Націнка 15% (Пікові години)
/// - 18:00 - 23:00: Знижка 20% (Вечірні години)
/// </summary>
public class PricingCalculator : IPricingCalculator
{
    private const decimal MorningDiscountMultiplier = 0.90m; // 10% знижка
    private const decimal PeakMarkupMultiplier = 1.15m;       // 15% націнка
    private const decimal EveningDiscountMultiplier = 0.80m;  // 20% знижка
    private const decimal StandardMultiplier = 1.00m;         // Стандартна ціна

    public (decimal TotalHallPrice, List<HourlyBreakdownDto> Breakdown) CalculateHallPrice(
        decimal basePricePerHour, 
        DateTime start, 
        TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero)
        {
            throw new ArgumentException("Тривалість бронювання повинна бути більшою за нуль.", nameof(duration));
        }

        var breakdown = new List<HourlyBreakdownDto>();
        decimal total = 0m;

        var currentCursor = start;
        var end = start + duration;

        while (currentCursor < end)
        {
            // Визначаємо кінець поточного кроку (по годинах або до фінального end)
            var nextHour = new DateTime(currentCursor.Year, currentCursor.Month, currentCursor.Day, currentCursor.Hour, 0, 0, currentCursor.Kind).AddHours(1);
            var stepEnd = nextHour < end ? nextHour : end;

            var fractionOfHour = (decimal)(stepEnd - currentCursor).TotalHours;

            var (multiplier, description) = GetRateForTime(currentCursor.TimeOfDay);

            var stepPrice = Math.Round(basePricePerHour * multiplier * fractionOfHour, 2, MidpointRounding.AwayFromZero);
            total += stepPrice;

            breakdown.Add(new HourlyBreakdownDto
            {
                StartTime = currentCursor,
                EndTime = stepEnd,
                RateMultiplier = multiplier,
                RateTypeDescription = description,
                CalculatedPrice = stepPrice
            });

            currentCursor = stepEnd;
        }

        return (total, breakdown);
    }

    /// <summary>
    /// Визначає коефіцієнт та опис для конкретного часу доби.
    /// </summary>
    private static (decimal Multiplier, string Description) GetRateForTime(TimeSpan timeOfDay)
    {
        var hours = timeOfDay.TotalHours;

        // Пікові години (12:00 - 14:00) мають найвищий пріоритет у денному проміжку
        if (hours >= 12.0 && hours < 14.0)
        {
            return (PeakMarkupMultiplier, "Пікові години (націнка 15%)");
        }

        // Ранкові години (06:00 - 09:00)
        if (hours >= 6.0 && hours < 9.0)
        {
            return (MorningDiscountMultiplier, "Ранкові години (знижка 10%)");
        }

        // Вечірні години (18:00 - 23:00)
        if (hours >= 18.0 && hours < 23.0)
        {
            return (EveningDiscountMultiplier, "Вечірні години (знижка 20%)");
        }

        // Стандартні години (09:00 - 18:00, за винятком 12:00-14:00) або інші
        return (StandardMultiplier, "Стандартні години (базова вартість)");
    }
}
