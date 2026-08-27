using Microsoft.AspNetCore.Mvc;
using ReserveSpace.Api.Models.DTOs.Common;
using ReserveSpace.Api.Models.DTOs.Rooms;
using ReserveSpace.Api.Services.Abstractions;

namespace ReserveSpace.Api.Controllers;

/// <summary>
/// Контролер для управління конференц-залами та пошуку доступних залів.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;
    private readonly ILogger<RoomsController> _logger;

    public RoomsController(IRoomService roomService, ILogger<RoomsController> logger)
    {
        _roomService = roomService;
        _logger = logger;
    }

    /// <summary>
    /// Отримати список усіх конференц-залів.
    /// </summary>
    /// <response code="200">Повертає список залів з послугами.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<RoomResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var rooms = await _roomService.GetAllAsync(cancellationToken);
        return Ok(rooms);
    }

    /// <summary>
    /// Отримати детальну інформацію про зал за ID.
    /// </summary>
    /// <param name="id">Унікальний ID залу.</param>
    /// <param name="cancellationToken">Токен скасування запиту.</param>
    /// <response code="200">Інформація про зал знайдена.</response>
    /// <response code="404">Зал не знайдено.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RoomResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var room = await _roomService.GetByIdAsync(id, cancellationToken);
        if (room == null)
        {
            return NotFound(new ErrorResponse($"Конференц-зал з ID {id} не знайдено."));
        }

        return Ok(room);
    }

    /// <summary>
    /// Пошук доступних конференц-залів за часом та місткістю (Метод 4 з ТЗ).
    /// </summary>
    /// <param name="request">Параметри пошуку: StartTime, EndTime, Capacity.</param>
    /// <param name="cancellationToken">Токен скасування запиту.</param>
    /// <response code="200">Повертає список доступних залів.</response>
    /// <response code="400">Помилка у переданих параметрах (наприклад, некоректний час).</response>
    [HttpGet("available")]
    [ProducesResponseType(typeof(IReadOnlyList<RoomResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAvailable([FromQuery] SearchAvailableRoomsRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var rooms = await _roomService.GetAvailableRoomsAsync(request, cancellationToken);
            return Ok(rooms);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Створити новий конференц-зал із списком доступних послуг (Метод 1 з ТЗ).
    /// </summary>
    /// <param name="request">Дані нового залу.</param>
    /// <param name="cancellationToken">Токен скасування запиту.</param>
    /// <response code="201">Зал успішно створено.</response>
    /// <response code="400">Помилка валідації вхідних даних.</response>
    [HttpPost]
    [ProducesResponseType(typeof(RoomResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateRoomRequestDto request, CancellationToken cancellationToken)
    {
        var created = await _roomService.CreateRoomAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Редагувати інформацію про зал та його послуги (Метод 2 з ТЗ).
    /// </summary>
    /// <param name="id">ID залу для оновлення.</param>
    /// <param name="request">Оновлені дані залу.</param>
    /// <param name="cancellationToken">Токен скасування запиту.</param>
    /// <response code="200">Зал успішно оновлено.</response>
    /// <response code="404">Зал не знайдено.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(RoomResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRoomRequestDto request, CancellationToken cancellationToken)
    {
        var updated = await _roomService.UpdateRoomAsync(id, request, cancellationToken);
        if (updated == null)
        {
            return NotFound(new ErrorResponse($"Конференц-зал з ID {id} не знайдено."));
        }

        return Ok(updated);
    }

    /// <summary>
    /// Видалити конференц-зал (Метод 3 з ТЗ).
    /// </summary>
    /// <param name="id">ID залу для видалення.</param>
    /// <param name="cancellationToken">Токен скасування запиту.</param>
    /// <response code="204">Зал успішно видалено.</response>
    /// <response code="404">Зал не знайдено.</response>
    /// <response code="400">Неможливо видалити зал через активні бронювання.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _roomService.DeleteRoomAsync(id, cancellationToken);
            if (!deleted)
            {
                return NotFound(new ErrorResponse($"Конференц-зал з ID {id} не знайдено."));
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponse(ex.Message));
        }
    }
}