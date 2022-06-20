using CardsHand.Card;

namespace CardsHand.Settings
{
    public interface ISettings
    {
        string ImagesRepositoryEndpoint { get; }

        ICardView CardView { get; }

        HandSettingsData HandSettingsData { get; }
    }
}