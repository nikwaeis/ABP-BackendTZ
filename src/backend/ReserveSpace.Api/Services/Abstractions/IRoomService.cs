using ReserveSpace.Api.Models.DTOs.Rooms;

namespace ReserveSpace.Api.Services.Abstractions;

/// <summary>
/// Сервіс для управління конференц-залами та пошуку доступності.
/// </summary>
public interface IRoomService
{
    /// <summary>
    /// Отримати зал за унікальним ідентифікатором.
    /// </summary>
    Task<RoomResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Отримати список усіх залів.
    /// </summary>
    Task<IReadOnlyList<RoomResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Пошук доступних залів за датою, часом та місткістю.
    /// </summary>
    Task<IReadOnlyList<RoomResponseDto>> GetAvailableRoomsAsync(SearchAvailableRoomsRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Створити новий конференц-зал.
    /// </summary>
    Task<RoomResponseDto> CreateRoomAsync(CreateRoomRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Оновити інформацію про конференц-зал.
    /// </summary>
    Task<RoomResponseDto?> UpdateRoomAsync(Guid id, UpdateRoomRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Видалити конференц-зал за ID.
    /// </summary>
    Task<bool> DeleteRoomAsync(Guid id, CancellationToken cancellationToken = default);
}