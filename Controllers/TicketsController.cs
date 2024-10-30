using Microsoft.AspNetCore.Mvc;
using YaTrackerParser.Interfaces;

namespace YaTrackerParser;

/// <summary>
/// Контроллер для получения файла с актуальными тикетами
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly IFileService _fileService;

    /// <summary>
    /// Контроллер
    /// </summary>
    /// <param name="fileService">Интерфейс сервиса FileService</param>
    public TicketsController(IFileService fileService)
    {
        _fileService = fileService;
    }

    /// <summary>
    /// Отправка файла по GET запросу
    /// </summary>
    /// <returns>Актуальный файл с тикетами</returns>
    [HttpGet("download")]
    public async Task<IActionResult> DownloadTicketsFile()
    {
        return await _fileService.GetTicketsFileAsync();
    }
}
