using UnityEngine;
using UnityEngine.UI;

namespace CardsHand.Settings
{
    public interface ISceneSettings
    {
        RectTransform RootUI { get; }
        RectTransform Hand { get; }

        Button InfluenceCardButton { get; }
    }
}