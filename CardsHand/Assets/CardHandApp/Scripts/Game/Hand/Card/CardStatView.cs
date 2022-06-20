using System;
using System.Threading;
using CardsHand.View;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CardsHand.CardHandApp.Scripts.Game.Hand.Card
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    [ExecuteInEditMode]
    public class CardStatView : BaseUICanvasGroupView
    {
        private const float _changeStatAnimationTime = 1f;

        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private FloatingNumberView _floatingNumberView;
        private CancellationTokenSource _cancellationTokenSource;

        private int _value;

        protected override void Awake()
        {
            base.Awake();
            _text = GetComponent<TextMeshProUGUI>();
        }

        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                _text.text = _value.ToString();
            }
        }

        public async UniTask UpdateStat(int newValue, CancellationToken token)
        {
            await UpdateStat(_value, newValue, token);
        }

        private async UniTask UpdateStat(int oldValue, int newValue, CancellationToken token)
        {
            if (oldValue != newValue)
            {
                int dir = newValue - oldValue;
                int stepsCount = Math.Abs(dir);

                int step = dir / stepsCount;

                float stepTime = _changeStatAnimationTime / stepsCount;

                FloatingNumber(dir);

                for (int i = 0; i < stepsCount; i++)
                {
                    await UniTask.Delay((int) (stepTime * 1000), DelayType.DeltaTime, PlayerLoopTiming.Update, token);
                    _value += step;
                    _text.text = _value.ToString();
                }
            }
        }

        private void FloatingNumber(int value)
        {
            _floatingNumberView.DOKill();
            _floatingNumberView.RectTransform.anchorMin = new Vector2(0, 1);
            _floatingNumberView.RectTransform.anchorMax = new Vector2(1, 2);

            _floatingNumberView.Value = value;
            _floatingNumberView.CanvasGroup.alpha = 1;
            _floatingNumberView.CanvasGroup.DOFade(0, _changeStatAnimationTime);
            _floatingNumberView.RectTransform.DOAnchorMin(new Vector2(0, 3), _changeStatAnimationTime);
            _floatingNumberView.RectTransform.DOAnchorMax(new Vector2(1, 4), _changeStatAnimationTime);
            _floatingNumberView.RectTransform.DOScale(Vector3.one * 2, _changeStatAnimationTime);
        }
    }
}