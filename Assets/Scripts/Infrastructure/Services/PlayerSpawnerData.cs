using System;
using StaticData;
using UnityEngine;

namespace Infrastructure.Services
{
    [Serializable]
    public class PlayerSpawnerData
    {
        public string Id;
        public PlayerTypeId PlayerTypeId;
        public Vector3 Position;

        public PlayerSpawnerData(string id, PlayerTypeId playerTypeId, Vector3 position)
        {
            Id = id;
            PlayerTypeId = playerTypeId;
            Position = position;
        }
    }
}