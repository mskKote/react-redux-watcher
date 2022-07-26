using Microsoft.AspNetCore.Mvc;
using ReactReduxWatcher.Services;
using ReactReduxWatcher.Services.NotifyServices;

namespace ReactReduxWatcher.Controllers;

[ApiController]
public sealed class  ReactReduxController : ControllerBase
{
    private readonly ILogger<ReactReduxController> _logger;
    private readonly IReactReduxRepoService _reactReduxRepo;
    private readonly INotifyerService _notifyer;
    
    public ReactReduxController(
        ILogger<ReactReduxController> logger, 
        IReactReduxRepoService reactReduxRepoService, 
        INotifyerService notifyer)
    {
        _logger = logger;
        _reactReduxRepo = reactReduxRepoService;
        _notifyer = notifyer;
    }

    [HttpPost("api/react-redux-webhook")]
    public async Task<string> ReactReduxWebhook()
    {
        var hasChangeStr = $"[{DateTime.Now:dd.MM.yyyy}] COMMIT FOR TRANSLATION";
        var hasNoChangeStr = $"[{DateTime.Now:dd.MM.yyyy}] Commit NOT suitable for translation";
        try
        {
            // Check commit
            var (hasDocsChange, lastCommit) = await _reactReduxRepo.CheckLastCommit();
            _logger.LogInformation(hasDocsChange ? hasChangeStr : hasNoChangeStr);
            if (!hasDocsChange) return hasNoChangeStr;

            // Notify translation team
            var isNotified = await _notifyer.Notify(lastCommit);
            return hasChangeStr;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            Response.StatusCode = 500;
            return $"ERROR // {e.Message}";
        }
    }
}