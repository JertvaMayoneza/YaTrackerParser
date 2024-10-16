using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YaTrackerParser.Models;
using YaTrackerParser.Services;
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

    public async Task ProcessTicketsAsync()
    {
        var issues = await _getTicketsService.GetTicketsAsync();
        var filteredTickets = _ticketFilterService.FilterTickets(issues);
        await _fileWriterService.WriteToFileAsync("filtered_tickets.txt", filteredTickets);
    }
}
