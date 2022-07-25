using ReactReduxDocsNotifyer.Model;

namespace ReactReduxDocsNotifyer.Services.NotifyServices;

public interface INotifyerService
{
    Task<bool> Notify(LastCommit commit);
}