using ReactReduxWatcher.Model;

namespace ReactReduxWatcher.Services.NotifyServices;

public interface INotifyerService
{
    Task<bool> Notify(LastCommit commit);
}