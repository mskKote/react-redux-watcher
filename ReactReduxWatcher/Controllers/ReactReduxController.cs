using Microsoft.AspNetCore.Mvc;
using ReactReduxDocsNotifyer.Services;
using ReactReduxDocsNotifyer.Services.NotifyServices;

namespace ReactReduxDocsNotifyer.Controllers;

/* 
 *TODO вебхук на репозитории
 *TODO → сервер отправляет запрос за последним коммитом
 * 
 *TODO |→ если в files были изменения на папке docs
 *TODO |→ происходит запрос к боту в определённый чат
 * 
 *TODO ||→ если изменений не было,
 *TODO ||→ то ничего не происходит 
 */


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
    public async Task ReactReduxWebhook()
    {
        try
        {
            var (hasDocsChange, lastCommit) = await _reactReduxRepo.CheckLastCommit();
            if (hasDocsChange)
            {
                
            }
            var isNotified = await _notifyer.Notify(lastCommit);
            _logger.LogInformation(isNotified ? 
                $"[{DateTime.Now:dd.MM.yyyy}] COMMIT FOR TRANSLATION" : 
                $"[{DateTime.Now:dd.MM.yyyy}] Commit NOT suitable for translation");

        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
    }
}