﻿using YaTrackerParser.Contracts.DTO.IssueModel;

namespace YaTrackerParser.Contracts.Interfaces;
/// <summary>
/// Интерфейс для запуска сервиса GetTickets
/// </summary>
public interface IGetTicketsService
{/// <summary>
/// Получение тикетов от Yandex API
/// </summary>
/// <returns>Список не отфильтрованных тикетов</returns>
    Task<List<Issue>> GetTicketsAsync();
}

