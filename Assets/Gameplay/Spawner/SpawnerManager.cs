using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Zlipacket.Core.Tools.Extension;
using Zlipacket.Core.Tools.Utilities;

namespace Gameplay.Spawner
{
    public class SpawnerManager : Singleton<SpawnerManager>
    {
        [Header("Configs")]
        public int currentWave = 0;
        public List<WaveInfo> waveInfos = new List<WaveInfo>();
        
        [Header("Components")]
        public Transform spawnParent;
        [SerializeField] public BoxCollider spawnBoxLeft;
        [SerializeField] public BoxCollider spawnBoxRight;
        
        public TextMeshProUGUI waveText;
        
        [Header("Events")]
        public UnityEvent<int> onWaveStarted;
        
        private Coroutine co_SpawnWave;
        public bool IsSpawning => co_SpawnWave != null;
        
        private Timer waveTimer;

        private void Start()
        {            
            if (waveText == null)
                return;
            
            onWaveStarted.AddListener((waveIndex) =>
            {
                waveText.SetText("Wave: " + (waveIndex + 1));
            });
        }

        private void Update()
        {
            waveTimer?.Tick(Time.deltaTime);
        }

        public void StartWave(int waveIndex)
        {
            onWaveStarted?.Invoke(currentWave);
            
            WaveInfo wave = waveInfos[Mathf.Max(0, waveInfos.Count - 1)];
            
            waveTimer = new Timer(wave.Duration);
            waveTimer.OnTimerComplete += NextWave;
            waveTimer.Start();
            
            if (IsSpawning)
                StopCoroutine(co_SpawnWave);
            
            co_SpawnWave = StartCoroutine(SpawningWave(wave));
        }

        private IEnumerator SpawningWave(WaveInfo waveInfo)
        {
            foreach (var spawnInfo in waveInfo.spawnInfos)
            {
                Spawn(spawnInfo);
                yield return new WaitForSeconds(spawnInfo.Delay);
            }
            
            co_SpawnWave = null;
        }

        private void Spawn(SpawnInfo spawnInfo)
        {
            BoxCollider spawnBox = UnityEngine.Random.Range(0, 2) == 0 ? spawnBoxLeft : spawnBoxRight;
            Vector3 spawnPoint = ZlipUtilities.GetRandomPointInBoxCollider(spawnBox).Insert(y: 0f);
            
            GameObject spawn = Instantiate(spawnInfo.prefab, spawnParent);
            spawn.transform.position = spawnPoint;
        }

        public void NextWave()
        {
            StartWave(currentWave++);
        }
    }
}