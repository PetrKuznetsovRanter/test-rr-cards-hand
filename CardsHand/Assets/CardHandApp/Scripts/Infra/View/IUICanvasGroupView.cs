using UnityEngine;

namespace CardsHand.View
{
    public interface IUICanvasGroupView : IUIView
    {
        CanvasGroup CanvasGroup { get; }
    }
}