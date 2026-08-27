using Microsoft.EntityFrameworkCore;
using ReserveSpace.Api.Data;
using ReserveSpace.Api.Domain.Entities;
using ReserveSpace.Api.Models.DTOs.Additions;
using ReserveSpace.Api.Models.DTOs.Rooms;
using ReserveSpace.Api.Services.Abstractions;

namespace ReserveSpace.Api.Services.Implementations;

/// <summary>
/// Реалізація сервісу управління конференц-залами.
/// </summary>
public class RoomService : IRoomService
{
    private readonly AppDbContext _context;
    private readonly ILogger<RoomService> _logger;

    public RoomService(AppDbContext context, ILogger<RoomService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<RoomResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var room = await _context.Rooms
            .AsNoTracking()
            .Include(r => r.Additions)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        return room == null ? null : MapToDto(room);
    }

    public async Task<IReadOnlyList<RoomResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var rooms = await _context.Rooms
            .AsNoTracking()
            .Include(r => r.Additions)
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);

        return rooms.Select(MapToDto).ToList();
    }

    public async Task<IReadOnlyList<RoomResponseDto>> GetAvailableRoomsAsync(SearchAvailableRoomsRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request.StartTime >= request.EndTime)
        {
            throw new ArgumentException("Час початку бронювання повинен бути раніше за час завершення.");
        }

        // Шукаємо ID залів, які зайняті в зазначений період (перетин інтервалів: StartTime < req.EndTime && EndTime > req.StartTime)
        var busyRoomIdsQuery = _context.Reservations
            .Where(res => res.StartTime < request.EndTime && res.EndTime > request.StartTime)
            .Select(res => res.RoomId)
            .Distinct();

        var availableRooms = await _context.Rooms
            .AsNoTracking()
            .Include(r => r.Additions)
            .Where(r => r.Capacity >= request.Capacity && !busyRoomIdsQuery.Contains(r.Id))
            .OrderBy(r => r.Capacity)
            .ToListAsync(cancellationToken);

        return availableRooms.Select(MapToDto).ToList();
    }

    public async Task<RoomResponseDto> CreateRoomAsync(CreateRoomRequestDto request, CancellationToken cancellationToken = default)
    {
        var room = new Rooms
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Capacity = request.Capacity,
            PricePerHour = request.PricePerHour,
            CreatedAt = DateTime.UtcNow,
            Additions = request.Additions?.Select(a => new Additions
            {
                Id = Guid.NewGuid(),
                Name = a.Name.Trim(),
                Price = a.Price,
                CreatedAt = DateTime.UtcNow
            }).ToList() ?? []
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Створено конференц-зал ID: {RoomId} з назвою: {RoomName}", room.Id, room.Name);

        return MapToDto(room);
    }

    public async Task<RoomResponseDto?> UpdateRoomAsync(Guid id, UpdateRoomRequestDto request, CancellationToken cancellationToken = default)
    {
        var room = await _context.Rooms
            .Include(r => r.Additions)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (room == null)
        {
            return null;
        }

        room.Name = request.Name.Trim();
        room.Capacity = request.Capacity;
        room.PricePerHour = request.PricePerHour;
        room.UpdatedAt = DateTime.UtcNow;

        if (request.Additions != null)
        {
            // Оновлюємо список послуг: видаляємо старі та додаємо нові
            _context.Additions.RemoveRange(room.Additions);
            room.Additions = request.Additions.Select(a => new Additions
            {
                Id = Guid.NewGuid(),
                RoomId = room.Id,
                Name = a.Name.Trim(),
                Price = a.Price,
                CreatedAt = DateTime.UtcNow
            }).ToList();
        }

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Оновлено конференц-зал ID: {RoomId}", room.Id);

        return MapToDto(room);
    }

    public async Task<bool> DeleteRoomAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var room = await _context.Rooms
            .Include(r => r.Reservations)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (room == null)
        {
            return false;
        }

        // Перевірка: якщо є майбутні бронювання, забороняємо видалення
        var hasFutureReservations = room.Reservations.Any(r => r.EndTime > DateTime.UtcNow);
        if (hasFutureReservations)
        {
            throw new InvalidOperationException("Неможливо видалити зал, для якого існують активні або майбутні бронювання.");
        }

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Видалено конференц-зал ID: {RoomId}", id);
        return true;
    }

    private static RoomResponseDto MapToDto(Rooms room)
    {
        return new RoomResponseDto
        {
            Id = room.Id,
            Name = room.Name,
            Capacity = room.Capacity,
            PricePerHour = room.PricePerHour,
            CreatedAt = room.CreatedAt,
            UpdatedAt = room.UpdatedAt,
            Additions = room.Additions.Select(a => new AdditionResponseDto
            {
                Id = a.Id,
                Name = a.Name,
                Price = a.Price
            }).ToList()
        };
    }
}
