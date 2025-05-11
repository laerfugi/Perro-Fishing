using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderingFishSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject wanderingNPCPrefab;
    public int maxNPCs = 5;

    [Header("Dependencies")]
    public Transform wanderAreaBounds;
    public Database database;

    [SerializeReference]
    private List<GameObject> spawnedNPCs = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < maxNPCs; i++)
        {
            SpawnWanderingNPC();
        }
    }

    void Update()
    {
        if (spawnedNPCs.Count < maxNPCs)
        {
            SpawnWanderingNPC();
        }
    }

    private void SpawnWanderingNPC()
    {
        Vector3 spawnPosition = GetRandomPositionWithinBounds();

        GameObject newNPC = Instantiate(wanderingNPCPrefab, spawnPosition, Quaternion.identity);

        WanderingContainer wanderingContainer = newNPC.GetComponent<WanderingContainer>();
        if (wanderingContainer != null)
        {
            wanderingContainer.wanderArea = wanderAreaBounds;
        }

        WanderingFishInteraction wanderingFishInteraction = newNPC.GetComponentInChildren<WanderingFishInteraction>();
        if (wanderingFishInteraction != null)
        {
            // Tell each fish it's item data, sprite, and spawner
            Fish_ItemData randomFishData = GetRandomFishData();
            wanderingFishInteraction.fishData = randomFishData;

            WanderingFishInteraction fishInteract = newNPC.GetComponentInChildren<WanderingFishInteraction>();
            if (fishInteract != null)
            {
                fishInteract.SetSprite(randomFishData.icon);
                fishInteract.wanderingContainer = wanderingContainer;

                fishInteract.spawner = this;
            }
        }

        spawnedNPCs.Add(newNPC);
    }

    private Vector3 GetRandomPositionWithinBounds()
    {
        if (wanderAreaBounds == null)
        {
            Debug.LogError("Wander area is not assigned!");
            return transform.position;
        }

        Bounds bounds = new Bounds(wanderAreaBounds.position, wanderAreaBounds.localScale);

        Vector3 randomPosition = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            wanderAreaBounds.position.y,
            Random.Range(bounds.min.z, bounds.max.z)
        );

        // Make sure position is on the NavMesh
        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        // Fallback
        return wanderAreaBounds.position;
    }

    private Fish_ItemData GetRandomFishData()
    {
        if (database == null || database.fishList.Count == 0)
        {
            Debug.LogError("Database is not assigned to WanderingContainerSpawner!");
            return null;
        }

        return database.fishList[Random.Range(0, database.fishList.Count)];
    }

    public void RemoveNPC(GameObject npc)
    {
        if (spawnedNPCs.Contains(npc))
        {
            spawnedNPCs.Remove(npc);
        }
    }
}