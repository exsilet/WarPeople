using Infrastructure.Services;

namespace StaticData
{
    public interface IStaticDataService : IService
    {
        public void Load();
        LevelStaticData ForLevel(string sceneKey);
        public PlayerStaticData ForPlayer(PlayerTypeId typeID);
    }
}