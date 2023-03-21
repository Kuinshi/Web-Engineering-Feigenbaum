using System.Collections.Generic;
using System.Linq;
using Fusion;
using Manager;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrateSpawner : MonoBehaviour
{
    [SerializeField] private List<NetworkPrefabRef> crates = new List<NetworkPrefabRef>();
    [SerializeField] private float minTime = 5;
    [SerializeField] private float maxTime = 20;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    

    private float lastSpawn = 0;
    private float nextSpawn;
    private float timeGoneBy;

    private void Start()
    {
        SetNextSpawn();
    }

    private void SetNextSpawn()
    {
        timeGoneBy = 0;
        nextSpawn = Random.Range(minTime, maxTime);
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
        NetworkRunnerManager.Instance.Runner.Spawn(
            crates[Random.Range(0, crates.Count)], 
            spawnpoint.position, spawnpoint.rotation);

    }
}
