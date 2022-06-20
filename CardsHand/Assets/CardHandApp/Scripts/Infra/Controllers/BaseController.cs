using System.Threading;

namespace CardsHand.Controllers
{
    public abstract class BaseController : IController
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        public CancellationToken DisposeToken => _cancellationTokenSource.Token;

        public virtual void Dispose()
        {
            if (!_cancellationTokenSource.IsCancellationRequested)
                _cancellationTokenSource.Cancel();

            _cancellationTokenSource.Dispose();
        }
    }
}