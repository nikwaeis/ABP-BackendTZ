namespace ReserveSpace.Api.Models.DTOs.Common;

/// <summary>
/// Стандартна відповідь для помилок API (RFC 7807 Problem Details / Error response).
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Короткий опис або код помилки.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Список детальних помилок або валідацій.
    /// </summary>
    public IReadOnlyList<string>? Details { get; set; }

    public ErrorResponse() { }

    public ErrorResponse(string message, IReadOnlyList<string>? details = null)
    {
        Message = message;
        Details = details;
    }
}
