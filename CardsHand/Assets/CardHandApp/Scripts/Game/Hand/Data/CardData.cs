using System;
using UnityEngine;

namespace CardsHand.Card
{
    [Serializable]
    public class CardData
    {
        public int Attack;
        public string Description;
        public int HealthPoint;
        public int Mana;

        [NonSerialized] public Sprite Picture;

        public string Title;
    }
}