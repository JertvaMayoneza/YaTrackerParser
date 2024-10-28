﻿using ClosedXML.Excel;
using System.Globalization;
using YaTrackerParser.Models;

namespace YaTrackerParser.Services
{
    /// <summary>
    /// Сервис для записи тикетов в Excel
    /// </summary>
    public class FileWriterService
    {
        private readonly string _filePath = @"C:\Users\mrasv\OneDrive\Рабочий стол\Отчет.xlsx";
        /// <summary>
        /// Метод для записи и обновления информации уже имеющихся тикетов в Excel
        /// </summary>
        /// <param name="tickets">Отфильтрованные по времени тикеты</param>
        /// <exception cref="FormatException">Неверный формат даты</exception>
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

                        var formatStrings = new[] { "dd.MM.yyyy HH:mm", "dd.MM.yyyy H:mm" };

                        if (!DateTime.TryParseExact(existingTimeString,
                                                    formatStrings,
                                                    CultureInfo.InvariantCulture,
                                                    DateTimeStyles.None,
                                                    out var existingDateTime) ||
                            !DateTime.TryParseExact(ticketTimeString,
                                                    formatStrings,
                                                    CultureInfo.InvariantCulture,
                                                    DateTimeStyles.None,
                                                    out var ticketDateTime))
                        {
                            throw new FormatException($"Неверный формат даты: {existingTimeString} или {ticketTimeString}");
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
                    worksheet.Row(2).InsertRowsAbove(newTickets.Count);
                    worksheet.Cell("A2").InsertData(newTickets);
                }

                workbook.Save();
            }
        }
    }
}
