﻿using YaTrackerParser.Models;

namespace YaTrackerParser.Interfaces;

/// <summary>
/// Интерфейс для реализации класса DatabaseWriterService
/// </summary>
public interface IDatabaseWriterService
{/// <summary>
/// Вызов метода WriteToDatabaseAsync
/// </summary>
/// <param name="tickets">Отфильтрованные тикеты</param>
/// <returns>Запись в БД</returns>
    Task WriteToDatabaseAsync(IEnumerable<TicketData> tickets);
}
