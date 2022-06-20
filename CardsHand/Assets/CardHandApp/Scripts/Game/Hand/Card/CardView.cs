using System;
using System.Threading;
using CardsHand.CardHandApp.Scripts.Game.Hand.Card;
using CardsHand.View;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CardsHand.Card
{
    public class CardView : BaseUICanvasGroupView, ICardView, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private const float _showAnimationTime = 0.5f;

        public event Action<RectTransform> DragEndedEvent;
        public event Action DragStartedEvent;

        [SerializeField] private CardStatView _mana;
        [SerializeField] private CardStatView _attack;
        [SerializeField] private CardStatView _healthPoints;

        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Image _picture;
        [SerializeField] private Image _highlighter;

        private CancellationTokenSource _cancellationTokenSource;
        private RectTransform _table;

        public int Mana => _mana.Value;

        public int Attack => _attack.Value;

        public int HealthPoints => _healthPoints.Value;

        public bool HighLighted
        {
            get => _highlighter.enabled;
            set => _highlighter.enabled = value;
        }

        public bool Interactable { get; set; }

        public async UniTask UpdateMana(int newValue, CancellationToken token)
        {
            await _mana.UpdateStat(newValue, token);
        }

        public async UniTask UpdateAttack(int newValue, CancellationToken token)
        {
            await _attack.UpdateStat(newValue, token);
        }

        public async UniTask UpdateHealthPoints(int newValue, CancellationToken token)
        {
            await _healthPoints.UpdateStat(newValue, token);
        }

        public void SetData(CardData cardData)
        {
            if (cardData != null)
            {
                _titleText.text = cardData.Title;
                _descriptionText.text = cardData.Description;
                _mana.Value = cardData.Mana;
                _attack.Value = cardData.Attack;
                _healthPoints.Value = cardData.HealthPoint;
                _picture.sprite = cardData.Picture;
                gameObject.name = cardData.Title;
            }
            else
            {
                throw new Exception("Card view setting data is null.");
            }
        }

        public async UniTask Show(CancellationToken token)
        {
            await CanvasGroup.DOFade(1, _showAnimationTime).ToUniTask(TweenCancelBehaviour.Kill, token);
            CanvasGroup.blocksRaycasts = true;
        }

        public async UniTask Hide(CancellationToken token)
        {
            await CanvasGroup.DOFade(0, _showAnimationTime).ToUniTask(TweenCancelBehaviour.Kill, token);
            CanvasGroup.blocksRaycasts = false;
        }

        public void DropOnTable(RectTransform table)
        {
            _table = table;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (Interactable)
                RectTransform.position += (Vector3) eventData.delta;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Interactable)
            {
                CanvasGroup.blocksRaycasts = false;
                HighLighted = true;
                _table = null;
                DragStartedEvent?.Invoke();
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (Interactable)
            {
                CanvasGroup.blocksRaycasts = true;
                HighLighted = false;

                if (_table != null)
                {
                    RectTransform.SetParent(_table);
                    RectTransform.SetAsLastSibling();
                }
                DragEndedEvent?.Invoke(_table);
            }
        }
    }
}