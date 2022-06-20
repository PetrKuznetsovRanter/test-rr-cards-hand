using System;
using System.Threading;
using CardsHand.Card;
using CardsHand.Controllers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CardsHand.Boostrap
{
    public interface ICardController : IViewController<CardData>
    {
        event Action<ICardController> CardLeaveHandEvent;
        event Action<ICardController> CardReturnInHandEvent;
        event Action<ICardController> CardDiedEvent;

        RectTransform RectTransform { get; }

        UniTask UpdateRandomStat(CancellationToken token);
    }
}