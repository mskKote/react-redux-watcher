using System.Text;
using Newtonsoft.Json;
using ReactReduxDocsNotifyer.Model;
using RestSharp;

namespace ReactReduxDocsNotifyer.Services;

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
        var url = _configuration.GetValue<string>("LastCommitURL");
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
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("------- COMMIT FILES -------");

        // return commit.Files.Any(x => x.FileName.StartsWith("/docs"));
        foreach (var file in commit.Files)
        {
            stringBuilder.AppendLine(file.FileName);
            // if (!file.FileName.StartsWith("/docs"))
            // {
            //     return false;
            // }
        }
        stringBuilder.AppendLine("----- COMMIT FILES END -----");
        _logger.LogInformation(stringBuilder.ToString());

        return true;
    }
    
    public async Task<(bool, LastCommit)> CheckLastCommit()
    {
        var lastCommit = await GetLastCommit();
        var hasDocsChange = HasDocumentationChange(lastCommit);
        return (hasDocsChange, lastCommit);
    }
}