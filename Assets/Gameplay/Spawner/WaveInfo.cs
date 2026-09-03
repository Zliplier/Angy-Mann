using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Spawner
{
    [CreateAssetMenu(menuName = "Gameplay/Spawner/WaveInfo", fileName = "Wave")]
    public class WaveInfo : ScriptableObject
    {
        public float mixDuration;
        public float maxDuration;
        public float Duration => UnityEngine.Random.Range(mixDuration, maxDuration);
        
        public List<SpawnInfo> spawnInfos;
    }

    [Serializable]
    public class SpawnInfo
    {
        public GameObject prefab;
        public float minDelay;
        public float maxDelay;
        public float Delay => UnityEngine.Random.Range(minDelay, maxDelay);
    }
}