using System.Threading;
using Cysharp.Threading.Tasks;

namespace CardsHand.Commands
{
    public interface IInstantiateViewCommand<T>
    {
        UniTask<T> Execute(CancellationToken token);
    }
}