using CardsHand.Card;
using CardsHand.Commands;
using CardsHand.Controllers;
using CardsHand.Hand;
using CardsHand.Settings;

namespace CardsHand.Boostrap
{
    public class GameController : BaseController, IGameController
    {
        private readonly ICardDataRepository _cardDataRepository;
        private readonly IHandController _handController;
        private readonly IInstantiateViewCommand<ICardView> _instantiateCardViewCommand;

        private readonly IPicturesRepository _picturesRepository;
        private readonly ISceneSettings _sceneSettings;
        private readonly ISettings _settings;

        public GameController(ISettings settings, ISceneSettings sceneSettings)
        {
            _settings = settings;
            _sceneSettings = sceneSettings;

            _instantiateCardViewCommand = new InstantiateViewCommand<ICardView>(_settings.CardView);
            _picturesRepository = new PicturesRepository(_settings);
            _cardDataRepository = new CardDataRepository(_settings.HandSettingsData.CardsData);

            _handController = new HandController(_instantiateCardViewCommand, _settings, _sceneSettings, _picturesRepository, _cardDataRepository);
        }

        public override void Dispose()
        {
            _handController.Dispose();
        }

        public async void Init()
        {
            await _handController.Init();
        }
    }
}