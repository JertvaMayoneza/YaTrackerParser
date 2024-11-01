using YaTrackerParser.Contracts.DTO;

namespace YaTrackerParser.Contracts.Interfaces;
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
