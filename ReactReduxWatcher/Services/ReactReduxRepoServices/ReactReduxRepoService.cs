using System.Text;
using Newtonsoft.Json;
using ReactReduxWatcher.Model;
using RestSharp;

namespace ReactReduxWatcher.Services;

public class ReactReduxRepoService : IReactReduxRepoService
{
    private readonly ILogger<ReactReduxRepoService> _logger;
    private readonly IConfiguration _configuration;
    
    public ReactReduxRepoService(
        ILogger<ReactReduxRepoService> logger,
        IConfiguration configuration)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /*
     * Request to
     * Returns JSON string with last commit
     */
    private async Task<LastCommit> GetLastCommit()
    {
        // Receive last commit
        var url = _configuration.GetValue<string>("LAST_COMMIT_URL");
        var client = new RestClient(url);
        var request = new RestRequest { Method = Method.Get };
        var response = await client.ExecuteAsync(request);
        if (response.Content == null)
            throw new HttpRequestException("response.Content is NULL");

        // Deserialize via Newtonsoft
        var lastCommit = JsonConvert.DeserializeObject<LastCommit>(response.Content, 
            new JsonSerializerSettings { DateFormatString = "DateTime.Now:dd.MM.yyyy" });
        if (lastCommit == null)
            throw new JsonSerializationException("Not suitable structure for /LastCommit/");

        return lastCommit;
    }

    /*
     * Checks if /doc folder is changed.
     */
    private bool HasDocumentationChange(LastCommit commit)
    {
        return commit.Files.Any(x => x.FileName.StartsWith("docs"));
    }
    
    public async Task<(bool, LastCommit)> CheckLastCommit()
    {
        var lastCommit = await GetLastCommit();
        var hasDocsChange = HasDocumentationChange(lastCommit);
        return (hasDocsChange, lastCommit);
    }
}