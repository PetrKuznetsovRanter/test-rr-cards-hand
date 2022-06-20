using CardsHand.Settings;
using UnityEngine;

namespace CardsHand.Boostrap
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private Settings.Settings _settings;
        [SerializeField] private SceneSettings _sceneSettings;

        private IGameController _gameController;

        private void Awake()
        {
            _gameController = new GameController(_settings, _sceneSettings);

            _gameController.Init();
        }

        private void OnDestroy()
        {
            _gameController.Dispose();
        }
    }
}