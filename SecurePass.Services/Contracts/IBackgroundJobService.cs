using System.Linq.Expressions;

namespace SecurePass.Services.Contracts;

public interface IBackgroundJobService
{
    void RegisterFireAndForgetJob<T>(Expression<Func<T, Task>> methodCall);
}

