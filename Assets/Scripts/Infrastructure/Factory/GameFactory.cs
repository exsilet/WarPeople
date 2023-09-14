using Infrastructure.AssetManagement;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assets;

        public GameFactory(IAssetProvider assets)
        {
            _assets = assets;
        }
        
        public void CreateHub()
        {
            _assets.Instantiate(AssetPath.HudPath);
        }
    }
}