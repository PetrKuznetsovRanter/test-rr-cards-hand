using System;
using System.Threading;
using CardsHand.View;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CardsHand.Card
{
    public interface ICardView : IUICanvasGroupView
    {
        event Action<RectTransform> DragEndedEvent;
        event Action DragStartedEvent;

        int Mana { get; }
        int Attack { get; }
        int HealthPoints { get; }
        bool HighLighted { get; set; }
        bool Interactable { get; set; }

        UniTask UpdateMana(int newValue, CancellationToken cancellationToken);
        UniTask UpdateAttack(int newValue, CancellationToken cancellationToken);
        UniTask UpdateHealthPoints(int newValue, CancellationToken cancellationToken);

        void SetData(CardData cardData);
        UniTask Show(CancellationToken token);
        UniTask Hide(CancellationToken token);
        void DropOnTable(RectTransform table);
    }
}