using YaTrackerParser.Models;

namespace YaTrackerParser.Services;

public class TicketProcessor
{
    private readonly GetTicketsService _getTicketsService;
    private readonly TicketFilterService _ticketFilterService;
    private readonly FileWriterService _fileWriterService;

    public TicketProcessor(GetTicketsService getTicketsService, TicketFilterService ticketFilterService, FileWriterService fileWriterService)
    {
        _getTicketsService = getTicketsService;
        _ticketFilterService = ticketFilterService;
        _fileWriterService = fileWriterService;
    }

    public async Task<IEnumerable<TicketData>> ProcessTicketsAsync()
    {
        var issues = await _getTicketsService.GetTicketsAsync();
        var filteredTickets = _ticketFilterService.FilterTickets(issues);

        var ticketDataList = filteredTickets.Select(issue => new TicketData
        {
            TicketNumber = issue.Key ?? "Не указано",
            Time = issue.UpdatedAt?.ToString("g") ?? "Не указано",
            Theme = issue.Summary ?? "Не указано"
        }).ToList();

        await _fileWriterService.WriteToExcelAsync(ticketDataList);

        return ticketDataList; // Возвращаем список тикетов
    }



}

