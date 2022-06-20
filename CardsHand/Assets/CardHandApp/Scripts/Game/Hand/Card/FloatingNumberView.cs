using CardsHand.View;
using TMPro;
using UnityEngine;

namespace CardsHand.CardHandApp.Scripts.Game.Hand.Card
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    [ExecuteInEditMode]
    public class FloatingNumberView : BaseUICanvasGroupView, IUICanvasGroupView
    {
        [SerializeField] private TextMeshProUGUI _text;
        private int _value;

        public int Value
        {
            get => _value;
            set
            {
                _value = value;

                if (_value > 0)
                    _text.text = $"+{_value}";
                else
                    _text.text = $"{_value}";
            }
        }
    }
}