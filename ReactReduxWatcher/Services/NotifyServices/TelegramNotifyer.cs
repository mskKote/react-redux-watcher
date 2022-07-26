using System.Text;
using ReactReduxWatcher.Model;
using RestSharp;

namespace ReactReduxWatcher.Services.NotifyServices;

public class TelegramNotifyer : INotifyerService
{
    private readonly ILogger<TelegramNotifyer> _logger;
    private readonly IConfiguration _configuration;

    public TelegramNotifyer(
        ILogger<TelegramNotifyer> logger,
        IConfiguration configuration
        )
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<bool> Notify(LastCommit commit)
    {
        var builtMessage = BuildMessage(commit);
        var builtUrl = BuildURL(builtMessage);
        _logger.LogInformation($"#TG_MESSAGE\n{builtMessage}\n");
        
        var client = new RestClient(builtUrl);
        var request = new RestRequest() {Method = Method.Post};
        var response = await client.ExecuteAsync(request);
        Console.WriteLine(response.Content);
        return true;
    }

    private string BuildURL(string message)
    {
        var tgUrl = new StringBuilder("https://api.telegram.org/");
        tgUrl.Append($"bot{_configuration.GetValue<string>("TG_BOT_TOKEN")}/sendMessage");
        tgUrl.Append($"?chat_id={_configuration.GetValue<string>("CHAT_ID")}");
        tgUrl.Append($"&text={message}");
        return tgUrl.ToString();
    }

    private string BuildMessage(LastCommit commit)
    {
        var message = new StringBuilder();
        message.AppendLine($"Поступили изменения в документации [{commit.Commit.Committer.Date}]");        
        message.AppendLine($"\nСообщение:\n`{commit.Commit.Message}`");
        // message.AppendLine($"URL: {commit.Url}\n");
        message.AppendLine("\nЗатронутые файлы:");
        foreach (var file in commit.Files.Where(x => x.FileName.Contains("docs")))
        {
            message.AppendLine($"○ {file.FileName}");
        }
        return message.ToString();
    }
}