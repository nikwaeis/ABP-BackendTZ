using System.ComponentModel.DataAnnotations;

namespace ReserveSpace.Api.Models.DTOs.Additions;

/// <summary>
/// DTO для створення або оновлення додаткової послуги.
/// </summary>
public class AdditionRequestDto
{
    /// <summary>
    /// Назва додаткової послуги (наприклад, "Проєктор", "Wi-Fi", "Звук").
    /// </summary>
    [Required(ErrorMessage = "Назва послуги є обов'язковою")]
    [MaxLength(30, ErrorMessage = "Максимальна довжина назви послуги - 30 символів")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Вартість послуги в гривнях.
    /// </summary>
    [Range(0, 1000000, ErrorMessage = "Ціна послуги не може бути від'ємною")]
    public decimal Price { get; set; }
}

/// <summary>
/// DTO для відображення додаткової послуги.
/// </summary>
public class AdditionResponseDto
{
    /// <summary>
    /// Унікальний ідентифікатор послуги.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Назва додаткової послуги.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Вартість послуги.
    /// </summary>
    public decimal Price { get; set; }
}
