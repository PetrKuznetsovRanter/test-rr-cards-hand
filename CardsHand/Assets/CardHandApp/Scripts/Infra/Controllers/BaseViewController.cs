using System.Threading;
using CardsHand.View;
using Cysharp.Threading.Tasks;

namespace CardsHand.Controllers
{
    public abstract class BaseViewController<T> : BaseController, IViewController<T>
    {
        protected T _data;
        protected IView _view;

        public abstract UniTask Show(CancellationToken token);

        public abstract UniTask Hide(CancellationToken token);

        public UniTask Init(T data, CancellationToken token)
        {
            _data = data;
            return UniTask.CompletedTask;
        }
    }
}