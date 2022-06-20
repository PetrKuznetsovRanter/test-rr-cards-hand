using System;
using CardsHand.Settings;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace CardsHand.Card
{
    public class PicturesRepository : IPicturesRepository
    {
        private const string endpoint = "https://picsum.photos/512/512";
        private readonly ISettings _settings;

        public PicturesRepository(ISettings settings)
        {
            _settings = settings;
        }

        public async UniTask<Sprite> Request()
        {
            Sprite result = null;
            UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(_settings.ImagesRepositoryEndpoint);
            UnityWebRequest request = await unityWebRequest.SendWebRequest().ToUniTask();

            if (request.result != UnityWebRequest.Result.ConnectionError && request.result != UnityWebRequest.Result.ProtocolError && request.result != UnityWebRequest.Result.DataProcessingError)
            {
                var texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
                result = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one / 2);
            }
            else
            {
                throw new Exception($"Couldn't get image from request: {endpoint}. Result: {request.result}");
            }

            return result;
        }
    }
}