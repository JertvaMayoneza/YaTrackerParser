﻿
using YaTrackerParser.Models;

namespace YaTrackerParser.Interfaces;
/// <summary>
/// Интерфейс для запуска TicketService, предназнченного для запуска сервисов для получения и записи тикетов
/// </summary>
public interface ITicketProcessor
{/// <summary>
/// 
/// </summary>
/// <returns></returns>
    Task<List<TicketData>> ProcessTicketsAsync();
}
