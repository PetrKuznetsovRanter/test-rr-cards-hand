using System;
using System.Threading;

namespace CardsHand.Controllers
{
    public interface IController : IDisposable
    {
        CancellationToken DisposeToken { get; }
    }
}