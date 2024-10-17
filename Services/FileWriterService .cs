using ClosedXML.Excel;
using YaTrackerParser.Models;
using System.Globalization;

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
                        row => row 
                    );

                var newTickets = new List<TicketData>();

                foreach (var ticket in tickets)
                {
                    if (existingTickets.TryGetValue(ticket.TicketNumber, out var existingRow))
                    {
                        var existingTimeString = existingRow.Cell(2).GetString().Trim();
                        var ticketTimeString = ticket.Time.Trim();

                        DateTime existingDateTime;
                        DateTime ticketDateTime;

                        if (!DateTime.TryParseExact(existingTimeString,
                                                    "dd.MM.yyyy HH:mm",
                                                    CultureInfo.InvariantCulture,
                                                    DateTimeStyles.None,
                                                    out existingDateTime) &&
                            !DateTime.TryParseExact(existingTimeString,
                                                    "dd.MM.yyyy H:mm",
                                                    CultureInfo.InvariantCulture,
                                                    DateTimeStyles.None,
                                                    out existingDateTime))
                        {
                            throw new FormatException($"Неверный формат даты: {existingTimeString}");
                        }

                        if (!DateTime.TryParseExact(ticketTimeString,
                                                    "dd.MM.yyyy HH:mm",
                                                    CultureInfo.InvariantCulture,
                                                    DateTimeStyles.None,
                                                    out ticketDateTime) &&
                            !DateTime.TryParseExact(ticketTimeString,
                                                    "dd.MM.yyyy H:mm",
                                                    CultureInfo.InvariantCulture,
                                                    DateTimeStyles.None,
                                                    out ticketDateTime))
                        {
                            throw new FormatException($"Неверный формат даты: {ticketTimeString}");
                        }

                        if (ticketDateTime > existingDateTime)
                        {
                            worksheet.Row(existingRow.RowNumber()).Delete();

                            newTickets.Add(ticket);
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
