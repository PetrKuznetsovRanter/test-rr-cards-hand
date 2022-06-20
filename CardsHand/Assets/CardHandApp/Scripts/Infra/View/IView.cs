using UnityEngine;

namespace CardsHand.View
{
    public interface IView
    {
        Transform Transform { get; }

        bool Active { get; set; }
    }
}