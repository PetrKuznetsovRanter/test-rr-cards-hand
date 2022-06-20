using System;
using System.Threading;
using CardsHand.View;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace CardsHand.Commands
{
    public class InstantiateViewCommand<T> : IInstantiateViewCommand<T> where T : IView
    {
        private readonly T _prefab;

        public InstantiateViewCommand(T prefab)
        {
            _prefab = prefab;

            if (_prefab == null)
                throw new Exception($"Prefab for type{typeof(T)} is null.");
        }

        public virtual async UniTask<T> Execute(CancellationToken token)
        {
            var result = Object.Instantiate(_prefab.Transform.gameObject).GetComponent<T>();
            return result;
        }
    }
}