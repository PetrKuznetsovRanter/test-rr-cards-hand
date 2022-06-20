using CardsHand.Controllers;
using Cysharp.Threading.Tasks;

namespace CardsHand.Hand
{
    public interface IHandController : IController
    {
        UniTask Init();
    }
}