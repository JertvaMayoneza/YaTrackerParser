using System.Text.RegularExpressions;
using YaTrackerParser.Contracts.Interfaces;
using YaTrackerParser.Contracts.DTO.IssueModel;

namespace YaTrackerParser.Services;

/// <summary>
/// Фильтрация полученных тикетов от YandexAPI
/// </summary>
public class TicketFilterService : ITicketFilterService
{
    /// <summary>
    /// Вывод отфильтрованных тикетов
    /// </summary>
    /// <param name="issues">Модели тикетов</param>
    /// <returns>Список отфильтрованных тикетов</returns>
    public List<Issue> FilterTickets(List<Issue> issues)
    {
        return issues
            .Where(issue => !string.IsNullOrEmpty(issue.Key) && issue.UpdatedAt != DateTime.MinValue)
            .Select(issue =>
            {
                issue.Description = CleanDescription(issue.Description);
                return issue;
            })
            .ToList();
    }

    /// <summary>
    /// Метод для очистки описания тикета
    /// </summary>
    /// <param name="description">Исходное описание тикета</param>
    /// <returns>Очистенное описание</returns>
    private string CleanDescription(string description)
    {
        if (string.IsNullOrEmpty(description))
            return description;

        return Regex.Replace(description, @"\s+", " ").Trim();
    }
}
