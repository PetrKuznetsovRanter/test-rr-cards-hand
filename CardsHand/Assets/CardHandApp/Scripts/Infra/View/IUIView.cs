using UnityEngine;

namespace CardsHand.View
{
    public interface IUIView : IView
    {
        RectTransform RectTransform { get; }
    }
}