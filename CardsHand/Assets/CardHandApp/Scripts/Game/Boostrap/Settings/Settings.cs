using CardsHand.Card;
using UnityEngine;

namespace CardsHand.Settings
{
    [CreateAssetMenu(menuName = "CardsHands/Settings", fileName = "Settings", order = 0)]
    public class Settings : ScriptableObject, ISettings
    {
        [SerializeField] private string _imagesRepositoryEndpoint;

        [Header("Hand settings")] [SerializeField]
        private CardView _cardView;

        [SerializeField] private HandSettingsData _handSettingsData;

        public string ImagesRepositoryEndpoint => _imagesRepositoryEndpoint;

        public ICardView CardView => _cardView;

        public HandSettingsData HandSettingsData => _handSettingsData;
    }
}