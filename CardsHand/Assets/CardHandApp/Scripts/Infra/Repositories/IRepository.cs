using Cysharp.Threading.Tasks;

namespace CardsHand.CardHandApp.Scripts.Infra.Repositories
{
    public interface IRepository<T>
    {
        UniTask<T> Request();
    }
}