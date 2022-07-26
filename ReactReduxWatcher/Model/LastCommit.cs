using Newtonsoft.Json;

namespace ReactReduxWatcher.Model;

public sealed record LastCommit(CommitInfo Commit, IEnumerable<CommitFile> Files, string Url);
public sealed record CommitFile(string FileName);
public sealed record CommitInfo(CommitCommitter Committer, string Message);
public sealed record CommitCommitter(
    [JsonProperty("date", Required = Required.Always)] DateTime Date, 
    string Name, 
    string Email); 

//DateTime Date,