using ReactReduxDocsNotifyer.Model;

namespace ReactReduxDocsNotifyer.Services;

public interface IReactReduxRepoService
{
    Task<(bool, LastCommit)> CheckLastCommit();
}