using Microsoft.AspNetCore.Mvc;
using ReserveSpace.Api.Models.DTOs.Reports;
using ReserveSpace.Api.Services.Abstractions;

namespace ReserveSpace.Api.Controllers;

/// <summary>
/// Контролер для бізнес-звітів та аналітики (Вимога 2 з ТЗ: Звіти та аналітика).
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ReportsController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReportsController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    /// <summary>
    /// Отримати зведений аналітичний звіт з виручки, завантаженості та популярності залів і послуг.
    /// </summary>
    /// <param name="fromUtc">Початок періоду аналітики (необов'язково).</param>
    /// <param name="toUtc">Кінець періоду аналітики (необов'язково).</param>
    /// <param name="cancellationToken">Токен скасування запиту.</param>
    /// <response code="200">Повертає сформований бізнес-звіт.</response>
    [HttpGet("analytics")]
    [ProducesResponseType(typeof(BusinessAnalyticsReportDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAnalytics(
        [FromQuery] DateTime? fromUtc, 
        [FromQuery] DateTime? toUtc, 
        CancellationToken cancellationToken)
    {
        var report = await _reservationService.GetAnalyticsReportAsync(fromUtc, toUtc, cancellationToken);
        return Ok(report);
    }
}
