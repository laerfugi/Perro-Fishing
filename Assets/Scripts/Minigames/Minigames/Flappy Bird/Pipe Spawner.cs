using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    public GameObject pipe;
    public Transform topSpawn;
    public Transform bottomSpawn;
    public float timer = 0;
    private bool spawnBool;

    private void OnEnable()
    {
        EventManager.Tick += SpawnPipe; 
    }
    private void OnDisable()
    {
        EventManager.Tick -= SpawnPipe;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnPipe()
    {
        if (Minigame.Instance.minigameState == MinigameState.Play)
        {
            if (spawnBool) { Instantiate(pipe, topSpawn); }
            else { Instantiate(pipe, bottomSpawn); }
            spawnBool = !spawnBool;
        }
    }
}
