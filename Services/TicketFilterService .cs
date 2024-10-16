using System.Collections.Generic;
using System.Linq;
using YaTrackerParser.Models;

namespace YaTrackerParser.Services
{
    public class TicketFilterService
    {
        public List<string> FilterTickets(List<Issue> issues)
        {
            return issues
                .Where(issue => !string.IsNullOrEmpty(issue.Key) && issue.UpdatedAt != DateTime.MinValue)
                .Select(issue => $"Key: {issue.Key}, Summary: {issue.Summary}, UpdatedAt: {issue.UpdatedAt}")
                .ToList();
        }
    }
}
