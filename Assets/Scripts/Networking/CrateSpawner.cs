using System.Collections.Generic;
using System.Linq;
using Fusion;
using Manager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Networking
{
    public class CrateSpawner : MonoBehaviour
    {
        [SerializeField] private List<NetworkPrefabRef> crates = new List<NetworkPrefabRef>();
        [SerializeField] private float baseTime = 12;
        [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

        private float nextSpawn;
        private float timeGoneBy;
        private int crateOrder;

        private void Start()
        {
            SetNextSpawn();
        }

        private void SetNextSpawn()
        {
            timeGoneBy = 0;
            nextSpawn = baseTime / PlayerRegistry.Jaeger.Count();
        }
    
        private void Update()
        {
            if (!NetworkRunnerManager.Instance.Runner.IsServer)
                return;
        
            timeGoneBy += Time.deltaTime;

            if (timeGoneBy <= nextSpawn)
                return;
        
            SetNextSpawn();
            SpawnCrate();
        
        }

        private void SpawnCrate()
        {
            Transform spawnpoint = spawnPoints[Random.Range(0, spawnPoints.Count())];
            NetworkRunnerManager.Instance.Runner.Spawn(crates[crateOrder], spawnpoint.position, spawnpoint.rotation);
            crateOrder += 1;
            if (crateOrder >= crates.Count)
                crateOrder = 0;
        }
    }
}
