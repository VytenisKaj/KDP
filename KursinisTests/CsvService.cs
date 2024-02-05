namespace KursinisTests;

// Replace with a service class to test.
public class CSVService : ICSVService
{
    private readonly IFileService _fileService;

    public CSVService(IFileService fileService)
    {
        _fileService = fileService;
    }
    public string Query(string query)
    {
        return "";
    }

}
