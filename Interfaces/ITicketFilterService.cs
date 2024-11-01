using YaTrackerParser.Models.IssueModel;

namespace YaTrackerParser.Interfaces;
/// <summary>
/// Интерфейс TicketFilterService
/// </summary>
public interface ITicketFilterService
{/// <summary>
/// Фильтрация тикетов после GetServiceTickets
/// </summary>
/// <param name="tickets">Не отфильтрованные тикеты от GetServiceTickets</param>
/// <returns>Отфильтрованные тикеты</returns>
    List<Issue> FilterTickets(List<Issue> tickets);
}
