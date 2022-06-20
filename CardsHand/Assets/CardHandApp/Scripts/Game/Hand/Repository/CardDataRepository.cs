using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CardsHand.Card
{
    public class CardDataRepository : ICardDataRepository
    {
        private readonly List<CardData> _cards;

        public CardDataRepository(List<CardData> cards)
        {
            _cards = new List<CardData>(cards);
        }

        public UniTask<CardData> Request()
        {
            CardData result = _cards[Random.Range(0, _cards.Count)];
            return UniTask.FromResult(result);
        }
    }
}