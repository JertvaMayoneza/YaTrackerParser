using ClosedXML.Excel;
using YaTrackerParser.Models;
using System.Globalization;

namespace YaTrackerParser.Services
{
    public class FileWriterService
    {
        private readonly string _filePath = @"C:\Users\mrasv\OneDrive\Рабочий стол\Отчет.xlsx";

        public void WriteToExcel(IEnumerable<TicketData> tickets)
        {
            if (!File.Exists(_filePath))
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Tickets");
                worksheet.Cell("A1").Value = "Номер заявки";
                worksheet.Cell("B1").Value = "Время";
                worksheet.Cell("C1").Value = "Тема";

                worksheet.Cell("A2").InsertData(tickets);

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

                        DateTime ticketDateTime;

                        var formatStrings = new string[] { "dd.MM.yyyy HH:mm", "dd.MM.yyyy H:mm" };

                        if (!DateTime.TryParseExact(existingTimeString,
                                                    formatStrings,
                                                    CultureInfo.InvariantCulture,
                                                    DateTimeStyles.None,
                                                    out var existingDateTime))
                        {
                            throw new FormatException($"Неверный формат даты: {existingTimeString}");
                        }

                        //TODO: сделать тоже самое как выше

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

                if (newTickets.Count > 0)
                {
                    int startRow = 2; 

                    worksheet.Row(startRow).InsertRowsAbove(newTickets.Count);

                    worksheet.Cell("A2").InsertData(newTickets);
                }

                workbook.Save();
            }
        }
    }
}
