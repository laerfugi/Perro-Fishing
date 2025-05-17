using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleGuySpawner : MonoBehaviour
{
    public GameObject littleGuyPrefab;
    public static LittleGuySpawner Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public GameObject CreateLittleGuy(Vector3 position, LittleGuy_ItemData littleGuyData)
    {
        GameObject littleGuy = Instantiate(littleGuyData.item, position, Quaternion.identity);
        return littleGuy;
    }

    public GameObject LoadLittleGuy(Vector3 position, LittleGuy_ItemData littleGuyData)
    {
        GameObject littleGuy = Instantiate(littleGuyData.item, position, Quaternion.identity);
        if(littleGuy.TryGetComponent(out LittleGuyNav nav))
        {
            nav.isFleeing = false;
        }
        return littleGuy;
    }
}
