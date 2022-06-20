using System.Collections.Generic;
using UnityEngine;

namespace CardsHand.Card
{
    [CreateAssetMenu(menuName = "CardsHands/HandSettings", fileName = "HandSettings", order = 0)]
    public class HandSettingsData : ScriptableObject
    {
        public float AngleBetweenCards;
        public float AnimationDuration;

        public List<CardData> CardsData;

        public float MaxHandAngle;

        public int MaxInfluenceOnCard;
        public int MaxInitialHandSize;

        public int MinInfluenceOnCard;
        public int MinInitialHandSize;
    }
}