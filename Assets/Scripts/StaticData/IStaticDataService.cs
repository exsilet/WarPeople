using Infrastructure.Services;

namespace StaticData
{
    public interface IStaticDataService : IService
    {
        public void Load();
        public PlayerStaticData ForPlayer(PlayerTypeId typeID);
        public PlayerStaticData ForEnemy(PlayerTypeId typeID);
        LevelStaticData ForLevel(string sceneKey);
    }
}