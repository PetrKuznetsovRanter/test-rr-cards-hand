using System;
using UnityEngine;

namespace CardsHand.View
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BaseUICanvasGroupView : BaseUIView, IUICanvasGroupView
    {
        protected override void Awake()
        {
            base.Awake();
            CanvasGroup = GetComponent<CanvasGroup>();
        }

        public CanvasGroup CanvasGroup { get; private set; }

        public new bool Active
        {
            get => Math.Abs(CanvasGroup.alpha - 1) < float.Epsilon;
            set => CanvasGroup.alpha = value ? 1 : 0;
        }
    }
}