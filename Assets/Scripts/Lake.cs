using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//An area that fish spawn in
public class Lake : MonoBehaviour
{
    [Header("Fish List")]
    public List<GameObject> fishList;
    public int maxFish;

    public GameObject fish;

    [Header("Spawn Areas")]
    public List<Transform> spawnAreaList;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < maxFish; i++)
        {
            SpawnFish();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fishList.Count < maxFish)
        {
            SpawnFish();
        }
    }

    void SpawnFish()
    {
        GameObject newFish = Instantiate(fish, spawnAreaList[Random.Range(0, spawnAreaList.Count)].position, fish.transform.rotation);
        newFish.transform.SetParent(this.transform);
        newFish.GetComponent<Fish>().lake = this;
        fishList.Add(newFish);
    }
}
