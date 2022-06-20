using System;
using System.Threading;
using CardsHand.Card;
using CardsHand.Commands;
using CardsHand.Controllers;
using CardsHand.Settings;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace CardsHand.Boostrap
{
    public class CardController : BaseViewController<CardView>, ICardController
    {
        public event Action<ICardController> CardLeaveHandEvent;
        public event Action<ICardController> CardReturnInHandEvent;
        public event Action<ICardController> CardDiedEvent;
        private readonly IInstantiateViewCommand<ICardView> _instantiateViewCommand;
        private readonly ISceneSettings _sceneSettings;
        private readonly ISettings _settings;

        public CardController(ISettings settings, ISceneSettings sceneSettings, IInstantiateViewCommand<ICardView> instantiateViewCommand)
        {
            _settings = settings;
            _sceneSettings = sceneSettings;
            _instantiateViewCommand = instantiateViewCommand;
        }

        private ICardView CardView { get; set; }

        public RectTransform RectTransform => CardView.RectTransform;

        public override void Dispose()
        {
            base.Dispose();
            CardView.DragEndedEvent -= OnDragEnded;
            CardView.DragStartedEvent -= OnDragStarted;

            try
            {
                if (CardView != null)
                {
                    Transform transform = CardView.Transform;

                    if (transform != null)
                        Object.Destroy(transform.gameObject);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public override async UniTask Show(CancellationToken token)
        {
            await CardView.Show(DisposeToken);
        }

        public override async UniTask Hide(CancellationToken token)
        {
            await CardView.Hide(DisposeToken);
        }

        public async UniTask Init(CardData cardData, CancellationToken token)
        {
            CardView = await _instantiateViewCommand.Execute(token);
            CardView.Interactable = true;
            CardView.DragEndedEvent += OnDragEnded;
            CardView.DragStartedEvent += OnDragStarted;
            CardView.SetData(cardData);
        }

        public async UniTask UpdateRandomStat(CancellationToken token)
        {
            CardView.Interactable = false;
            int value = Random.Range(_settings.HandSettingsData.MinInfluenceOnCard, _settings.HandSettingsData.MaxInfluenceOnCard - 1);

            if (value == 0)
            {
                value = _settings.HandSettingsData.MaxInfluenceOnCard;
            }

            float previousAnchorPoxY = RectTransform.anchoredPosition.y;
            Quaternion previousRotation = RectTransform.rotation;

            await UniTask.WhenAll(RectTransform.DOAnchorPosY(previousAnchorPoxY + RectTransform.rect.height * 2, _settings.HandSettingsData.AnimationDuration).ToUniTask(TweenCancelBehaviour.Kill, DisposeToken),
                RectTransform.DORotateQuaternion(Quaternion.identity, _settings.HandSettingsData.AnimationDuration).ToUniTask(TweenCancelBehaviour.Kill, DisposeToken));

            switch (Random.Range(0, 3))
            {
                case 0:
                {
                    await CardView.UpdateAttack(CardView.Attack + value, token);
                    break;
                }
                case 1:
                {
                    await CardView.UpdateMana(CardView.Mana + value, token);
                    break;
                }
                case 2:
                {
                    await CardView.UpdateHealthPoints(CardView.HealthPoints + value, token);

                    break;
                }
            }

            await UniTask.WhenAll(RectTransform.DOAnchorPosY(previousAnchorPoxY, _settings.HandSettingsData.AnimationDuration).ToUniTask(TweenCancelBehaviour.Kill, DisposeToken),
                RectTransform.DORotateQuaternion(previousRotation, _settings.HandSettingsData.AnimationDuration).ToUniTask(TweenCancelBehaviour.Kill, DisposeToken));

            if (CheckHealthPoints())
            {
                CardView.Interactable = true;
            }
        }

        private void OnDragStarted()
        {
            RectTransform.DORotateQuaternion(Quaternion.identity, _settings.HandSettingsData.AnimationDuration);
            CardLeaveHandEvent?.Invoke(this);
        }

        private void OnDragEnded(RectTransform rectTransform)
        {
            if (rectTransform == null)
            {
                CardReturnInHandEvent?.Invoke(this);
            }
        }

        private bool CheckHealthPoints()
        {
            if (CardView.HealthPoints <= 0)
            {
                CardDiedEvent?.Invoke(this);
                return false;
            }

            return true;
        }
    }
}