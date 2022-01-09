using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NomaNova.Ojeda.Services.Shared.Background.Interfaces;

public interface IJobService
{
    void Fire(Expression<Action> methodCall);

    void Fire(Expression<Func<Task>> methodCall);

    void Delay(Expression<Action> methodCall, TimeSpan delay);

    void Delay(Expression<Func<Task>> methodCall, TimeSpan delay);
}