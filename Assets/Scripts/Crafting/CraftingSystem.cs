using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CraftingSystem : MonoBehaviour
{
    private DatabaseWrapper databaseWrapper;
    private RecipeDisplayManager recipeDisplayManager;
    private CraftingUIManager craftingUIManager;
    public Transform spawnPoint;
    public Transform spawnAreaBounds;
    void Awake()
    {
        databaseWrapper = GetComponent<DatabaseWrapper>();
        RecipeBook.Initialize(databaseWrapper);
    }

    void Start()
    {
        recipeDisplayManager = GetComponent<RecipeDisplayManager>();
        craftingUIManager = GetComponent<CraftingUIManager>();
    }

    public void Craft()
    {
        //(Material_ItemData mat1, Material_ItemData mat2) = (recipeDisplayManager.currentCraft[0], recipeDisplayManager.currentCraft[1]);
        //Debug.Log($"{recipeDisplayManager.currentCraft}");
        if (!HasRequiredMaterials(recipeDisplayManager.currentCraft[0], recipeDisplayManager.currentCraft[1]))
        {
            Debug.LogWarning("Missing materials.");
            //return null;
            return;
        }

        LittleGuy_ItemData littleGuyData = RecipeBook.UseRecipe(recipeDisplayManager.currentCraft[0], recipeDisplayManager.currentCraft[1]);
        // Failed recipe
        if (littleGuyData == null)
        {
            Debug.LogWarning("Invalid recipe");
            //return null;
            return;
        }

        // Recipe success
        ConsumeMaterials(recipeDisplayManager.currentCraft[0], recipeDisplayManager.currentCraft[1]);
        recipeDisplayManager.OnOpen(); // refresh the UI
        craftingUIManager.ResetCraftingUI();
        Vector3 runToPosition = GetNearestNavMeshPosition(GetRandomPointInSpawnBounds());
        Debug.Log($"attempting to spawn little guy at {runToPosition}");
        // Spawn little guy through giving data
        Vector3 originalSpawn = GetNearestNavMeshPosition(spawnPoint.position);
        GameObject craftedItem = LittleGuySpawner.Instance.CreateLittleGuy(originalSpawn, littleGuyData);

        Debug.Log("trying to get little guy in craft");
        if (craftedItem.TryGetComponent(out LittleGuy littleGuy))
        {
            Debug.Log("trying to get little guy nav mesh in craft");
            if (craftedItem.TryGetComponent(out LittleGuyNav nav))
            {
                Debug.Log("got nav, trying to set its stats");
                StartCoroutine(runNavInitialTarget(nav));
                //nav.SetUncatchable(true);
                //nav.SetSpeed(12f, 20f);
                //Vector3 runTarget = GetRandomPointInSpawnBounds();
                //nav.RunToInitialTarget(runTarget, () =>
                //{
                //    nav.SetUncatchable(false);
                //    nav.SetSpeed(4f, 4f); // flee speeds
                //    nav.isFleeing = true;
                //});
            }
        }
    }

    public IEnumerator runNavInitialTarget(LittleGuyNav nav)
    {
        yield return new WaitForEndOfFrame();
        nav.SetUncatchable(true);
        nav.SetSpeed(12f, 20f);
        Vector3 runTarget = GetNearestNavMeshPosition(GetRandomPointInSpawnBounds());
        nav.RunToInitialTarget(runTarget, () =>
        {
            nav.SetUncatchable(false);
            nav.SetSpeed(4f, 4f); // flee speeds
            nav.isFleeing = true;
        });
    }

    private Vector3 GetRandomPointInSpawnBounds()
    {
        Vector3 center = spawnAreaBounds.position;
        Vector3 areaSize = spawnAreaBounds.localScale;
        float x = Random.Range(center.x - areaSize.x / 2, center.x + areaSize.x / 2);
        float y = center.y;
        float z = Random.Range(center.z - areaSize.z / 2, center.z + areaSize.z / 2);

        //Debug.Log($"random point in spawn bounds set to {new Vector3(x, y, z)}");
        return new Vector3(x, y, z);
    }

    private bool HasRequiredMaterials(ItemData material1, ItemData material2)
    {
        var wrapper1 = PlayerInventory.Instance.InInventory(material1);
        var wrapper2 = PlayerInventory.Instance.InInventory(material2);

        return wrapper1 != null && wrapper1.count > 0 &&
               wrapper2 != null && wrapper2.count > 0;
    }

    private void ConsumeMaterials(ItemData material1, ItemData material2)
    {
        PlayerInventory.Instance.RemoveItem(material1);
        PlayerInventory.Instance.RemoveItem(material2);
    }

    private Vector3 GetNearestNavMeshPosition(Vector3 origin)
    {
        if (NavMesh.SamplePosition(origin, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return Vector3.zero;
    }

}
