using System.IO;
using System.Threading.Tasks;

namespace YaTrackerParser.Services
{
    public class FileWriterService
    {
        public async Task WriteToFileAsync(string filePath, IEnumerable<string> lines)
        {
            await File.WriteAllLinesAsync(filePath, lines);
        }
    }
}
