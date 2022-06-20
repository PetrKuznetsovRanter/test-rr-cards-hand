using System.Threading;
using Cysharp.Threading.Tasks;

namespace CardsHand.Controllers
{
    public interface IViewController<T> : IController
    {
        UniTask Show(CancellationToken token);
        UniTask Hide(CancellationToken token);

        UniTask Init(T data, CancellationToken token);
    }
}