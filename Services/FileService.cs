using Microsoft.AspNetCore.Mvc;
using YaTrackerParser.Interfaces;

namespace YaTrackerParser.Services;

/// <summary>
/// Класс для отправки актуального файла с тикетами
/// </summary>
public class FileService : IFileService
{
    private readonly string _filePath = "C:\\Users\\elkin\\Documents\\tickets.xlsx"; // Не забыть поменять

    /// <summary>
    /// Метод для отправки актуального файла с тикетами
    /// </summary>
    /// <returns>Файл с тикетами, либо сообщение</returns>
    public async Task<ActionResult> GetTicketsFileAsync()
    {
        if (!File.Exists(_filePath))
        {
            return new NotFoundObjectResult("Файл отсутствует.");
        }

        var fileInfo = new FileInfo(_filePath);

        if (fileInfo.Length == 0)
        {
            return new OkObjectResult("Нет новых тикетов.");
        }

        var timestamp = DateTime.Now.ToString("dd/MM_HH/tmm");
        var fileNameWithDate = $"{Path.GetFileNameWithoutExtension(_filePath)}_{timestamp}{fileInfo.Extension}";
        var newFilePath = Path.Combine(fileInfo.DirectoryName, fileNameWithDate);

        File.Copy(_filePath, newFilePath, true);

        var fileBytes = await File.ReadAllBytesAsync(newFilePath);

        return new FileContentResult(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            FileDownloadName = fileNameWithDate
        };
    }
}
