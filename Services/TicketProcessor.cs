using YaTrackerParser.Interfaces;
using YaTrackerParser.Models;

namespace YaTrackerParser.Services;

/// <summary>
/// Класс для запуска сервисов
/// </summary>
public class TicketProcessor : ITicketProcessor
{
    private readonly IGetTicketsService _igetTicketsService;
    private readonly ITicketFilterService _iticketFilterService;
    private readonly IFileWriterService _ifileWriterService;
    private readonly IDatabaseWriterService _idatabaseWriterService;

    /// <summary>
    /// Создание экземпляра TicketProcessor
    /// </summary>
    /// <param name="getTicketsService"></param>
    /// <param name="ticketFilterService"></param>
    /// <param name="databaseWriterService"></param>
    /// <param name="fileWriterService"></param>
    public TicketProcessor(IGetTicketsService getTicketsService,
                            ITicketFilterService ticketFilterService,
                            IDatabaseWriterService databaseWriterService,
                            IFileWriterService fileWriterService)
    {
        _igetTicketsService = getTicketsService;
        _iticketFilterService = ticketFilterService;
        _idatabaseWriterService = databaseWriterService;
        _ifileWriterService = fileWriterService;
    }

    /// <summary>
    /// Метод для запуска необходимых сервисов
    /// </summary>
    /// <returns></returns>
    public async Task<List<TicketData>> ProcessTicketsAsync()
    {
        var issues = await _igetTicketsService.GetTicketsAsync();

        var filteredTickets = _iticketFilterService.FilterTickets(issues);

        var ticketDataList = filteredTickets
            .Select(issue => new TicketData
            {
                TicketNumber = issue.Key ?? "Не указано",
                Time = issue.UpdatedAt?.ToString("g") ?? "Не указано",
                Theme = issue.Summary ?? "Не указано",
                Description = issue.Description ?? "Не указано",
                UpdatedBy = issue.UpdatedBy.Display ?? "Не указано"
            })
            .ToList();

        await _idatabaseWriterService.WriteToDatabaseAsync(ticketDataList);

        _ifileWriterService.WriteToExcel(ticketDataList);

        return ticketDataList;
    }
}
