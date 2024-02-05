

namespace KursinisTests;

public interface IFileService
{
    IEnumerable<string> ReadFileLines(string fileName);
    string ReadFile(string fileName);
}

