using ReactReduxWatcher.Model;

namespace ReactReduxWatcher.Services;

public interface IReactReduxRepoService
{
    Task<(bool, LastCommit)> CheckLastCommit();
}