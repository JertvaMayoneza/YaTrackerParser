using YaTrackerParser.Models;

namespace YaTrackerParser.Services
{
    public class TicketFilterService
    {
        public List<Issue> FilterTickets(List<Issue> issues)
        {
            return issues
                .Where(issue => !string.IsNullOrEmpty(issue.Key) && issue.UpdatedAt != DateTime.MinValue)
                .ToList();
        }
    }
}
