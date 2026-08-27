using Microsoft.EntityFrameworkCore;
using ReserveSpace.Api.Data;
using ReserveSpace.Api.Domain.Entities;
using ReserveSpace.Api.Models.DTOs.Additions;
using ReserveSpace.Api.Models.DTOs.Reports;
using ReserveSpace.Api.Models.DTOs.Reservations;
using ReserveSpace.Api.Services.Abstractions;

namespace ReserveSpace.Api.Services.Implementations;

/// <summary>
/// Реалізація сервісу для створення бронювань та формування аналітичних звітів.
/// </summary>
public class ReservationService : IReservationService
{
    private readonly AppDbContext _context;
    private readonly IPricingCalculator _pricingCalculator;
    private readonly ILogger<ReservationService> _logger;

    public ReservationService(
        AppDbContext context, 
        IPricingCalculator pricingCalculator, 
        ILogger<ReservationService> logger)
    {
        _context = context;
        _pricingCalculator = pricingCalculator;
        _logger = logger;
    }

    public async Task<ReservationResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var reservation = await _context.Reservations
            .AsNoTracking()
            .Include(r => r.Room)
            .Include(r => r.ReservationAdditions)
                .ThenInclude(ra => ra.Addition)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (reservation == null)
        {
            return null;
        }

        var (_, breakdown) = _pricingCalculator.CalculateHallPrice(
            reservation.Room.PricePerHour, 
            reservation.StartTime, 
            reservation.Duration);

        return new ReservationResponseDto
        {
            Id = reservation.Id,
            RoomId = reservation.RoomId,
            RoomName = reservation.Room.Name,
            StartTime = reservation.StartTime,
            Duration = reservation.Duration,
            EndTime = reservation.EndTime,
            HallPrice = reservation.HallPrice,
            AdditionsPrice = reservation.AdditionsPrice,
            TotalPrice = reservation.TotalPrice,
            CreatedAt = reservation.CreatedAt,
            PriceBreakdown = breakdown,
            SelectedAdditions = reservation.ReservationAdditions.Select(ra => new AdditionResponseDto
            {
                Id = ra.Addition.Id,
                Name = ra.Addition.Name,
                Price = ra.Addition.Price
            }).ToList()
        };
    }

    public async Task<ReservationResponseDto> CreateReservationAsync(CreateReservationRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request.Duration <= TimeSpan.Zero)
        {
            throw new ArgumentException("Тривалість бронювання повинна бути більшою за 0 хвилин.");
        }

        var endTime = request.StartTime + request.Duration;

        // 1. Отримуємо інформацію про зал та його доступні послуги
        var room = await _context.Rooms
            .Include(r => r.Additions)
            .FirstOrDefaultAsync(r => r.Id == request.RoomId, cancellationToken);

        if (room == null)
        {
            throw new KeyNotFoundException($"Конференц-зал з ID {request.RoomId} не знайдено.");
        }

        // 2. Перевіряємо відсутність перетинів за часом (Conflict validation)
        var isSlotTaken = await _context.Reservations
            .AnyAsync(r => r.RoomId == request.RoomId 
                        && r.StartTime < endTime 
                        && r.EndTime > request.StartTime, cancellationToken);

        if (isSlotTaken)
        {
            throw new InvalidOperationException("Обраний зал вже заброньований на цей часовий інтервал.");
        }

        // 3. Розрахунок вартості оренди залу
        var (hallPrice, breakdown) = _pricingCalculator.CalculateHallPrice(room.PricePerHour, request.StartTime, request.Duration);

        // 4. Перевірка та розрахунок вартості обраних послуг
        var selectedAdditions = new List<Additions>();
        decimal additionsPrice = 0m;

        if (request.SelectedAdditionIds != null && request.SelectedAdditionIds.Count > 0)
        {
            var requestedIds = request.SelectedAdditionIds.Distinct().ToList();
            selectedAdditions = room.Additions.Where(a => requestedIds.Contains(a.Id)).ToList();

            if (selectedAdditions.Count != requestedIds.Count)
            {
                throw new ArgumentException("Одна або декілька обраних послуг недоступні для цього залу.");
            }

            additionsPrice = selectedAdditions.Sum(a => a.Price);
        }

        var totalPrice = hallPrice + additionsPrice;

        // 5. Зберігаємо бронювання в БД
        var reservationId = Guid.NewGuid();
        var reservation = new Reservations
        {
            Id = reservationId,
            RoomId = room.Id,
            StartTime = request.StartTime,
            Duration = request.Duration,
            EndTime = endTime,
            HallPrice = hallPrice,
            AdditionsPrice = additionsPrice,
            TotalPrice = totalPrice,
            CreatedAt = DateTime.UtcNow,
            ReservationAdditions = selectedAdditions.Select(a => new ReservationAdditions
            {
                Id = Guid.NewGuid(),
                ReservationId = reservationId,
                AdditionId = a.Id
            }).ToList()
        };

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Успішно створено бронювання ID: {ReservationId} для залу {RoomName}. Загальна вартість: {TotalPrice} грн", 
            reservation.Id, room.Name, totalPrice);

        return new ReservationResponseDto
        {
            Id = reservation.Id,
            RoomId = room.Id,
            RoomName = room.Name,
            StartTime = reservation.StartTime,
            Duration = reservation.Duration,
            EndTime = reservation.EndTime,
            HallPrice = hallPrice,
            AdditionsPrice = additionsPrice,
            TotalPrice = totalPrice,
            CreatedAt = reservation.CreatedAt,
            PriceBreakdown = breakdown,
            SelectedAdditions = selectedAdditions.Select(a => new AdditionResponseDto
            {
                Id = a.Id,
                Name = a.Name,
                Price = a.Price
            }).ToList()
        };
    }

    public async Task<BusinessAnalyticsReportDto> GetAnalyticsReportAsync(DateTime? fromUtc, DateTime? toUtc, CancellationToken cancellationToken = default)
    {
        var query = _context.Reservations
            .AsNoTracking()
            .Include(r => r.Room)
            .Include(r => r.ReservationAdditions)
                .ThenInclude(ra => ra.Addition)
            .AsQueryable();

        if (fromUtc.HasValue)
        {
            query = query.Where(r => r.StartTime >= fromUtc.Value);
        }

        if (toUtc.HasValue)
        {
            query = query.Where(r => r.EndTime <= toUtc.Value);
        }

        var reservations = await query.ToListAsync(cancellationToken);

        var totalCount = reservations.Count;
        var hallRevenue = reservations.Sum(r => r.HallPrice);
        var additionsRevenue = reservations.Sum(r => r.AdditionsPrice);
        var totalRevenue = reservations.Sum(r => r.TotalPrice);
        var totalHours = reservations.Sum(r => r.Duration.TotalHours);
        var avgCheck = totalCount > 0 ? Math.Round(totalRevenue / totalCount, 2) : 0m;

        // Статистика залів
        var roomStats = reservations
            .GroupBy(r => new { r.RoomId, r.Room.Name })
            .Select(g => new RoomPopularityReportDto
            {
                RoomId = g.Key.RoomId,
                RoomName = g.Key.Name,
                ReservationsCount = g.Count(),
                TotalHoursBooked = Math.Round(g.Sum(x => x.Duration.TotalHours), 2),
                TotalRevenue = g.Sum(x => x.TotalPrice)
            })
            .OrderByDescending(x => x.TotalRevenue)
            .ToList();

        // Статистика послуг
        var additionStats = reservations
            .SelectMany(r => r.ReservationAdditions)
            .GroupBy(ra => new { ra.AdditionId, ra.Addition.Name, ra.Addition.Price })
            .Select(g => new AdditionPopularityReportDto
            {
                AdditionId = g.Key.AdditionId,
                AdditionName = g.Key.Name,
                OrderCount = g.Count(),
                TotalRevenue = g.Count() * g.Key.Price
            })
            .OrderByDescending(x => x.OrderCount)
            .ToList();

        return new BusinessAnalyticsReportDto
        {
            TotalRevenue = totalRevenue,
            HallRevenue = hallRevenue,
            AdditionsRevenue = additionsRevenue,
            TotalReservationsCount = totalCount,
            TotalBookedHours = Math.Round(totalHours, 2),
            AverageCheck = avgCheck,
            RoomStats = roomStats,
            AdditionStats = additionStats
        };
    }
}
