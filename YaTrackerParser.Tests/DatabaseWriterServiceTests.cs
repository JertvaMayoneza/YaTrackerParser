using DocumentFormat.OpenXml.Spreadsheet;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using YaTrackerParser.Contracts.DTO;
using YaTrackerParser.Contracts.DTO.IssueModel;
using YaTrackerParser.Contracts.Interfaces;
using YaTrackerParser.Services;

public class TicketProcessorTests
{
    [Fact]
    public async Task ProcessTicketsAsync_ShouldCallWriteToDatabaseAsync_WhenValidTicketsArePassed()
    {
        var mockGetTicketsService = new Mock<IGetTicketsService>();
        var mockTicketFilterService = new Mock<ITicketFilterService>();
        var mockDatabaseWriterService = new Mock<IDatabaseWriterService>();
        var mockFileWriterService = new Mock<IFileWriterService>();

        var mockIssues = new List<Issue>
    {
        new Issue { Key = "TICKET-123", Summary = "Theme1", Description = "Description1", UpdatedAt = DateTime.Now, UpdatedBy = new UpdatedByInfo { Display = "User1" } },
        new Issue { Key = "TICKET-124", Summary = "Theme2", Description = "Description2", UpdatedAt = DateTime.Now, UpdatedBy = new UpdatedByInfo { Display = "User2" } },
        new Issue { Key = null, Summary = "Theme3", Description = "Description3", UpdatedAt = DateTime.MinValue, UpdatedBy = new UpdatedByInfo { Display = "User3" } }
    };

        mockGetTicketsService.Setup(service => service.GetTicketsAsync())
            .ReturnsAsync(mockIssues);

        mockTicketFilterService.Setup(service => service.FilterTickets(It.IsAny<List<Issue>>()))
            .Returns(mockIssues.Where(ticket => !string.IsNullOrWhiteSpace(ticket.Key)).ToList());

        var ticketProcessor = new TicketProcessor(
            mockGetTicketsService.Object,
            mockTicketFilterService.Object,
            mockDatabaseWriterService.Object,
            mockFileWriterService.Object
        );

        await ticketProcessor.ProcessTicketsAsync();

        mockDatabaseWriterService.Verify(service => service.WriteToDatabaseAsync(It.Is<List<TicketData>>(tickets => tickets.Count == 2)), Times.Once);
    }

}
