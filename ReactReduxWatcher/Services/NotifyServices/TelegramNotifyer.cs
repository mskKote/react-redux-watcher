using ReactReduxDocsNotifyer.Model;

namespace ReactReduxDocsNotifyer.Services.NotifyServices;

public class TelegramNotifyer : INotifyerService
{
    public Task<bool> Notify(LastCommit commit)
    {
        // TODO: запрос к боту
        // TODO: выписать ID чата
        // TODO: создать бота
        return Task.FromResult(true);
    }
}