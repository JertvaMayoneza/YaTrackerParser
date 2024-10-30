using YaTrackerParser.Interfaces;
using YaTrackerParser.Models;

namespace YaTrackerParser.Services
{   /// <summary>
    /// Фильтрация полученных тикетов от YandexAPI
    /// </summary>
    public class TicketFilterService : ITicketFilterService
    {   /// <summary>
        /// Вывод фильтрованных тикетов
        /// </summary>
        /// <param name="issues">Модели тикетов</param>
        /// <returns></returns>
        public List<Issue> FilterTickets(List<Issue> issues)
        {
            return issues
                .Where(issue => !string.IsNullOrEmpty(issue.Key) && issue.UpdatedAt != DateTime.MinValue)
                .ToList();
        }
    }
}
