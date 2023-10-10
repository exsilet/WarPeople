using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure.AssetManagement
{
    public interface IAssetProvider : IService
    {
        GameObject Instantiate(string path);
        GameObject Instantiate(string path, Vector3 at);
        public GameObject Instantiate(string path, string pathPosition);
        GameObject InstantiatePhoton(string path, string pathPosition);
        public GameObject InstantiatePhotonRoom(string path, string pathPosition);
    }
}