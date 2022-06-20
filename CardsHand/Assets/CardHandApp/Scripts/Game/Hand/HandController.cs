using System.Collections.Generic;
using System.Threading;
using CardsHand.Boostrap;
using CardsHand.Card;
using CardsHand.Commands;
using CardsHand.Controllers;
using CardsHand.Settings;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace CardsHand.Hand
{
    public class HandController : BaseController, IHandController
    {
        private readonly List<ICardController> _cardControllers;
        private readonly ICardDataRepository _cardDataRepository;

        private readonly List<Sprite> _cardsImagesCache;

        private readonly RectTransform _handContainer;
        private readonly IInstantiateViewCommand<ICardView> _instantiateViewCommand;
        private readonly IPicturesRepository _picturesRepository;
        private readonly ISceneSettings _sceneSettings;
        private readonly ISettings _settings;

        private int _cardCursor;

        public HandController(IInstantiateViewCommand<ICardView> instantiateViewCommand, ISettings settings, ISceneSettings sceneSettings, IPicturesRepository picturesRepository, ICardDataRepository cardDataRepository)
        {
            _settings = settings;
            _sceneSettings = sceneSettings;
            _picturesRepository = picturesRepository;
            _cardDataRepository = cardDataRepository;

            _handContainer = _sceneSettings.Hand;

            _instantiateViewCommand = instantiateViewCommand;
            _cardControllers = new List<ICardController>();

            _cardsImagesCache = new List<Sprite>();

            _cardDataRepository = cardDataRepository;

            _sceneSettings.InfluenceCardButton.onClick.AddListener(OnClickInfluenceOnCard);
            _cardCursor = 0;
        }

        public override void Dispose()
        {
            base.Dispose();
            _sceneSettings.InfluenceCardButton.onClick.RemoveListener(OnClickInfluenceOnCard);

            foreach (ICardController cardController in _cardControllers)
            {
                cardController.CardDiedEvent -= OnCardDied;
                cardController.CardLeaveHandEvent -= OnCardLeaveHand;
                cardController.CardReturnInHandEvent -= OnCardReturnInHand;
                cardController.Dispose();
            }
        }

        public async UniTask Init()
        {
            _sceneSettings.InfluenceCardButton.interactable = false;

            await RequestImages();

            await GenerateCards();

            _sceneSettings.InfluenceCardButton.interactable = true;
        }

        private void OnClickInfluenceOnCard()
        {
            UpdateCardsStats();
        }

        private async void UpdateCardsStats()
        {
            _sceneSettings.InfluenceCardButton.interactable = false;

            while (_cardControllers.Count > 0)
            {
                _cardCursor = (_cardCursor + 1) % _cardControllers.Count;
                ICardController cardController = _cardControllers[_cardCursor];
                await cardController.UpdateRandomStat(DisposeToken);
            }

            _sceneSettings.InfluenceCardButton.interactable = true;
        }

        private async UniTask GenerateCards()
        {
            for (int i = 0; i < Random.Range(_settings.HandSettingsData.MinInitialHandSize, _settings.HandSettingsData.MaxInitialHandSize); i++)
            {
                CardData cardData = await _cardDataRepository.Request();
                var cardController = new CardController(_settings, _sceneSettings, _instantiateViewCommand);

                cardData.Picture = _cardsImagesCache[i];

                await cardController.Init(cardData, DisposeToken);

                cardController.RectTransform.anchoredPosition = Vector2.zero;
                cardController.RectTransform.SetParent(_handContainer, false);

                _cardControllers.Add(cardController);
                cardController.CardDiedEvent += OnCardDied;
                cardController.CardLeaveHandEvent += OnCardLeaveHand;
                cardController.CardReturnInHandEvent += OnCardReturnInHand;

                await UniTask.WhenAll(cardController.Show(DisposeToken), UpdateHand(DisposeToken), CardSetInHandSequence(cardController.RectTransform).Play().ToUniTask(TweenCancelBehaviour.Kill, DisposeToken));
            }
        }

        private async void OnCardReturnInHand(ICardController cardController)
        {
            _cardControllers.Add(cardController);
            cardController.RectTransform.SetParent(_sceneSettings.Hand);
            cardController.RectTransform.SetAsLastSibling();
            await UpdateHand(DisposeToken);
        }

        private async void OnCardLeaveHand(ICardController cardController)
        {
            _cardControllers.Remove(cardController);
            cardController.RectTransform.SetParent(_sceneSettings.RootUI);
            cardController.RectTransform.SetAsLastSibling();
            await UpdateHand(DisposeToken);
        }

        private async void OnCardDied(ICardController cardController)
        {
            _cardControllers.Remove(cardController);
            cardController.CardDiedEvent -= OnCardDied;
            cardController.CardLeaveHandEvent -= OnCardLeaveHand;
            cardController.CardReturnInHandEvent -= OnCardReturnInHand;
            cardController.Dispose();
            await UpdateHand(DisposeToken);
        }

        private async UniTask UpdateHand(CancellationToken cancellationToken)
        {
            var cardsAnimation = new List<UniTask>();

            float angleStep = 0;
            float startAngle = 0;
            float maxAngle = _settings.HandSettingsData.AngleBetweenCards * _cardControllers.Count;

            if (_settings.HandSettingsData.MaxHandAngle > maxAngle)
            {
                angleStep = _settings.HandSettingsData.AngleBetweenCards;
            }
            else
            {
                maxAngle = _settings.HandSettingsData.MaxHandAngle;
                angleStep = maxAngle / _cardControllers.Count;
            }

            startAngle = maxAngle / 2 - angleStep / 2;

            for (int j = 0; j < _cardControllers.Count; j++)
            {
                var rotation = Quaternion.Euler(0, 0, startAngle - j * angleStep);

                cardsAnimation.Add(_cardControllers[j].RectTransform
                    .DOAnchorPos(rotation * new Vector2(0, _handContainer.rect.height - _cardControllers[j].RectTransform.rect.height / 2), _settings.HandSettingsData.AnimationDuration)
                    .ToUniTask(TweenCancelBehaviour.Kill, cancellationToken));
                cardsAnimation.Add(_cardControllers[j].RectTransform.DORotateQuaternion(rotation, _settings.HandSettingsData.AnimationDuration).ToUniTask(TweenCancelBehaviour.Kill, cancellationToken));
            }

            await UniTask.WhenAll(cardsAnimation);
        }

        private Sequence CardSetInHandSequence(RectTransform rectTransform)
        {
            var result = DOTween.Sequence();
            result.Insert(0, rectTransform.DOAnchorMin(new Vector2(0.5f, 0), _settings.HandSettingsData.AnimationDuration));
            result.Insert(0, rectTransform.DOAnchorMax(new Vector2(0.5f, 0), _settings.HandSettingsData.AnimationDuration));
            result.Insert(0, rectTransform.DOPivot(new Vector2(0.5f, 0.5f), _settings.HandSettingsData.AnimationDuration));

            return result;
        }

        private async UniTask RequestImages()
        {
            for (int i = 0; i < _settings.HandSettingsData.MaxInitialHandSize; i++)
            {
                _cardsImagesCache.Add(await _picturesRepository.Request());
            }
        }
    }
}