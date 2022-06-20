using System;
using UnityEngine;
using UnityEngine.UI;

namespace CardsHand.Settings
{
    [Serializable]
    public class SceneSettings : ISceneSettings
    {
        [SerializeField] private RectTransform _rootUI;
        [SerializeField] private RectTransform _hand;
        [SerializeField] private Button _influenceCardButton;

        public RectTransform RootUI => _rootUI;
        public RectTransform Hand => _hand;

        public Button InfluenceCardButton => _influenceCardButton;
    }
}