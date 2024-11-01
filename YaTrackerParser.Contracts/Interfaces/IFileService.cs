using Microsoft.AspNetCore.Mvc;

namespace YaTrackerParser.Contracts.Interfaces;

/// <summary>
/// Интерфейс сервиса FileService
/// </summary>
public interface IFileService
{
    /// <summary>
    /// Возвращает файл с тикетами, если он существует и не пуст.
    /// </summary>
    /// <returns>Файл с тикетами или сообщение об ошибке.</returns>
    Task<ActionResult> GetTicketsFileAsync();
}
