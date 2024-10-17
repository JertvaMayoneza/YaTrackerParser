using ClosedXML.Excel;
using YaTrackerParser.Models;

namespace YaTrackerParser.Services
{
    public class FileWriterService
    {
        private readonly string _filePath = @"C:\Users\mrasv\OneDrive\Рабочий стол\Отчет.xlsx";

        public async Task WriteToExcelAsync(IEnumerable<TicketData> tickets)
        {
            if (!File.Exists(_filePath))
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Tickets");
                worksheet.Cell("A1").Value = "Номер заявки";
                worksheet.Cell("B1").Value = "Время";
                worksheet.Cell("C1").Value = "Тема";

                await WriteTicketsToWorksheet(worksheet, tickets);

                workbook.SaveAs(_filePath);
            }
            else
            {
                using var workbook = new XLWorkbook(_filePath);
                var worksheet = workbook.Worksheet(1);

                var existingTickets = worksheet.RowsUsed()
                    .Skip(1)
                    .ToDictionary(
                        row => row.Cell(1).GetString(),
                        row => row.Cell(2).GetString()
                    );

                var newTickets = new List<TicketData>();

                foreach (var ticket in tickets)
                {
                    if (existingTickets.TryGetValue(ticket.TicketNumber, out string existingDate))
                    {
                        if (string.Compare(ticket.Time, existingDate, StringComparison.Ordinal) > 0)
                        {
                            var rowToUpdate = worksheet.RowsUsed()
                                .First(row => row.Cell(1).GetString() == ticket.TicketNumber);

                            rowToUpdate.Cell(2).Value = ticket.Time;
                        }
                    }
                    else
                    {
                        newTickets.Add(ticket);
                    }
                }

                if (newTickets.Any())
                {
                    int startRow = 2;

                    int rowCount = worksheet.LastRowUsed()?.RowNumber() ?? 1;
                    worksheet.Row(startRow).InsertRowsAbove(newTickets.Count);

                    for (int i = 0; i < newTickets.Count; i++)
                    {
                        var ticket = newTickets[i];
                        worksheet.Cell(startRow + i, 1).Value = ticket.TicketNumber;
                        worksheet.Cell(startRow + i, 2).Value = ticket.Time;
                        worksheet.Cell(startRow + i, 3).Value = ticket.Theme;
                    }
                }

                workbook.Save();
            }
        }

        private async Task WriteTicketsToWorksheet(IXLWorksheet worksheet, IEnumerable<TicketData> tickets)
        {
            int row = worksheet.LastRowUsed()?.RowNumber() + 1 ?? 2;

            foreach (var ticket in tickets)
            {
                worksheet.Cell(row, 1).Value = ticket.TicketNumber;
                worksheet.Cell(row, 2).Value = ticket.Time;
                worksheet.Cell(row, 3).Value = ticket.Theme;
                row++;
            }
        }
    }
}
