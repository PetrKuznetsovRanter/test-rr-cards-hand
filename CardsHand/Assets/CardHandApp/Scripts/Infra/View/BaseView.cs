using UnityEngine;

namespace CardsHand.View
{
    public class BaseView : MonoBehaviour, IView
    {
        public Transform Transform => transform;

        public bool Active
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }
    }
}