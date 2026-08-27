using Microsoft.AspNetCore.Mvc;
using ReserveSpace.Api.Models.DTOs.Common;
using ReserveSpace.Api.Models.DTOs.Reservations;
using ReserveSpace.Api.Services.Abstractions;

namespace ReserveSpace.Api.Controllers;

/// <summary>
/// Контролер для створення та перегляду бронювань конференц-залів.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly ILogger<ReservationsController> _logger;

    public ReservationsController(
        IReservationService reservationService, 
        ILogger<ReservationsController> logger)
    {
        _reservationService = reservationService;
        _logger = logger;
    }

    /// <summary>
    /// Отримати деталі бронювання за ID.
    /// </summary>
    /// <param name="id">ID бронювання.</param>
    /// <param name="cancellationToken">Токен скасування запиту.</param>
    /// <response code="200">Бронювання знайдено.</response>
    /// <response code="404">Бронювання не знайдено.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ReservationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var reservation = await _reservationService.GetByIdAsync(id, cancellationToken);
        if (reservation == null)
        {
            return NotFound(new ErrorResponse($"Бронювання з ID {id} не знайдено."));
        }

        return Ok(reservation);
    }

    /// <summary>
    /// Забронювати конференц-зал із розрахунком вартості (Метод 5 з ТЗ).
    /// </summary>
    /// <param name="request">Параметри бронювання: RoomId, StartTime, Duration, SelectedAdditionIds.</param>
    /// <param name="cancellationToken">Токен скасування запиту.</param>
    /// <response code="201">Бронювання успішно створено, повертає розрахунок вартості.</response>
    /// <response code="400">Помилка у вхідних даних або невірна тривалість.</response>
    /// <response code="404">Зал не знайдено.</response>
    /// <response code="409">Зал вже заброньований на вказаний час (конфлікт).</response>
    [HttpPost]
    [ProducesResponseType(typeof(ReservationResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateReservationRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _reservationService.CreateReservationAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ErrorResponse(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            // Помилка конфлікту зайнятості слоту
            return Conflict(new ErrorResponse(ex.Message));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ErrorResponse(ex.Message));
        }
    }
}
