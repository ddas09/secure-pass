using Hangfire;
using System.Linq.Expressions;
using SecurePass.Services.Contracts;

namespace SecurePass.Services;

public class BackgroundJobService : IBackgroundJobService
{
    public void RegisterFireAndForgetJob<T>(Expression<Func<T, Task>> methodCall)
    {
        BackgroundJob.Enqueue<T>(methodCall);
    }
}

