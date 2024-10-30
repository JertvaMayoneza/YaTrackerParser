using YaTrackerParser.Models;

namespace YaTrackerParser.Interfaces;
/// <summary>
/// Интерфейс для записи тикетов в Excel
/// </summary>
public interface IFileWriterService
{/// <summary>
/// 
/// </summary>
/// <param name="tickets"></param>
    void WriteToExcel(IEnumerable<TicketData> tickets);
}
