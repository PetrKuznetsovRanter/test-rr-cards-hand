using UnityEngine;

namespace CardsHand.View
{
    [RequireComponent(typeof(RectTransform))]
    public class BaseUIView : BaseView, IUIView
    {
        protected virtual void Awake()
        {
            RectTransform = transform as RectTransform;
        }

        public RectTransform RectTransform { get; private set; }
    }
}