using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

public class MaterialSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject materialPrefab;
    public List<Material_ItemData> materialDataList;
    public Transform materialsParent;
    public int spawnCountPerMaterial = 5;
    public float respawnDelay = 10f;
    public float spawnYOffset = 1f;
    public float spawnCheckRadius = 0.5f;

    private Transform spawnerArea;
    private Dictionary<Vector3, Material_ItemData> spawnPointData = new Dictionary<Vector3, Material_ItemData>();
    private Dictionary<Vector3, GameObject> activeMaterials = new Dictionary<Vector3, GameObject>();

    public Transform exclusionZoneParent;
    private List<Collider> exclusionZones = new List<Collider>();

    void Start()
    {
        spawnerArea = transform.Find("SpawnerArea");
        if (spawnerArea == null)
        {
            Debug.LogError("SpawnerArea child transform not found!");
            return;
        }
        InitializeExclusionZones();

        PrecomputeSpawnPoints();

        SpawnMaterials();
    }

    void InitializeExclusionZones()
    {
        exclusionZones.Clear();

        if (exclusionZoneParent != null)
        {
            foreach (Transform child in exclusionZoneParent)
            {
                Collider collider = child.GetComponent<Collider>();
                if (collider != null)
                {
                    exclusionZones.Add(collider);
                }
            }
        }
    }

    void PrecomputeSpawnPoints()
    {
        Vector3 areaSize = spawnerArea.localScale;

        foreach (Material_ItemData materialData in materialDataList)
        {
            for (int i = 0; i < spawnCountPerMaterial; i++)
            {
                Vector3 spawnPoint = GetValidSpawnPoint(areaSize);
                if (spawnPoint != Vector3.zero)
                {
                    spawnPointData[spawnPoint] = materialData;
                }
                else
                {
                    Debug.LogWarning($"Could not find a valid spawn point for {materialData.name}");
                }
            }
        }
    }

    Vector3 GetValidSpawnPoint(Vector3 areaSize)
    {
        for (int attempts = 0; attempts < 70; attempts++)
        {
            Vector3 randomPosition = GetRandomPositionWithinArea(areaSize);

            if (Physics.Raycast(randomPosition + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 20f))
            {
                if (hit.collider != null)
                {
                    float spawnY = hit.point.y + spawnYOffset;
                    // If the spawned material is too far from the original transform
                    if (Mathf.Abs(spawnY - spawnerArea.position.y) <= 3f)
                    {
                        Vector3 potentialSpawnPoint = hit.point + Vector3.up * spawnYOffset;

                        // Check if it is trying to spawn inside a collider
                        if (!Physics.CheckSphere(potentialSpawnPoint, spawnCheckRadius) && !IsInsideExclusionZone(potentialSpawnPoint))
                        {
                            return potentialSpawnPoint;
                        } else
                        {
                            //Debug.Log("spawned inside a collider!");
                        }
                    }
                }
            }
        }

        Debug.LogError("Failed to find a valid spawn point within 70 attempts.");
        return Vector3.zero;
    }

    Vector3 GetRandomPositionWithinArea(Vector3 areaSize)
    {
        Vector3 center = spawnerArea.position;
        float x = Random.Range(center.x - areaSize.x / 2, center.x + areaSize.x / 2);
        float y = center.y;
        float z = Random.Range(center.z - areaSize.z / 2, center.z + areaSize.z / 2);

        return new Vector3(x, y, z);
    }

    void SpawnMaterials()
    {
        foreach (var entry in spawnPointData)
        {
            Vector3 spawnPoint = entry.Key;
            Material_ItemData materialData = entry.Value;

            SpawnMaterial(materialData, spawnPoint);
        }
    }

    void SpawnMaterial(Material_ItemData materialData, Vector3 spawnPoint)
    {
        GameObject materialInstance = Instantiate(materialPrefab, spawnPoint, Quaternion.identity, materialsParent);

        RespawnableItem respawnableItem = materialInstance.GetComponent<RespawnableItem>();
        if (respawnableItem != null)
        {
            // Set respawnable item data
            respawnableItem.itemData = materialData;
            respawnableItem.Initialize(this, spawnPoint);
        }

        SpriteRenderer spriteRenderer = materialInstance.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = materialData.icon;
        }

        activeMaterials[spawnPoint] = materialInstance;
    }

    bool IsInsideExclusionZone(Vector3 position)
    {
        foreach (var zone in exclusionZones)
        {
            if (zone.bounds.Contains(position))
            {
                return true;
            }
        }
        return false;
    }

    public void NotifyMaterialCollected(Vector3 spawnPoint, Material_ItemData materialData)
    {
        if (activeMaterials.ContainsKey(spawnPoint))
        {
            activeMaterials[spawnPoint] = null; // Mark the material as collected
            StartCoroutine(RespawnMaterialAfterDelay(spawnPoint, materialData));
        }
    }

    private IEnumerator RespawnMaterialAfterDelay(Vector3 spawnPoint, Material_ItemData materialData)
    {
        yield return new WaitForSeconds(respawnDelay);

        if (activeMaterials[spawnPoint] == null) // Check if the material hasn't been spawned yet
        {
            SpawnMaterial(materialData, spawnPoint);
        }
    }
}