using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hangfire;
using NomaNova.Ojeda.Services.Shared.Background.Interfaces;

namespace NomaNova.Ojeda.Services.Shared.Background;

public class JobService : IJobService
{
    private readonly IBackgroundJobClient _backgroundJobClient;

    public JobService(IBackgroundJobClient backgroundJobClient)
    {
        _backgroundJobClient = backgroundJobClient;
    }

    public void Fire(Expression<Func<Task>> methodCall)
    {
        _backgroundJobClient.Enqueue(methodCall);
    }

    public void Fire(Expression<Action> methodCall)
    {
        _backgroundJobClient.Enqueue(methodCall);
    }

    public void Delay(Expression<Func<Task>> methodCall, TimeSpan delay)
    {
        _backgroundJobClient.Schedule(methodCall, delay);
    }

    public void Delay(Expression<Action> methodCall, TimeSpan delay)
    {
        _backgroundJobClient.Schedule(methodCall, delay);
    }
}