using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class framerateSet : MonoBehaviour
{
    [Range(-1, 300)]
    public int framerate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Application.targetFrameRate = framerate;
    }
}
