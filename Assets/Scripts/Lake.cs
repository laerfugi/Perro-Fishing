using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            GameObject newFish = Instantiate(fish, spawnAreaList[Random.Range(0, spawnAreaList.Count)].position, Quaternion.identity);
            newFish.GetComponent<Fish>().lake = this;
            fishList.Add(newFish);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fishList.Count < maxFish)
        {
            GameObject newFish = Instantiate(fish, spawnAreaList[Random.Range(0, spawnAreaList.Count)].position, Quaternion.identity);
            newFish.GetComponent<Fish>().lake = this;
            fishList.Add(newFish);
        }
    }
}
