using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aimlabs_Aiming : MonoBehaviour
{
    Transform tr;
    Camera cam;
    bool started;
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        //Cursor.lockState = CursorLockMode.Locked;
        cam = GetComponent<Camera>();
        tr.rotation = new Quaternion(0, 0, 0, 0);
        started = false;
        Minigame.Instance.Lose();
    }

    // Update is called once per frame
    void Update()
    {
        if (!started)
        {
            if (Minigame.Instance.minigameState == MinigameState.Play)
            {
                started = true;
            }
            else
            {
                tr.rotation = new Quaternion(0, 0, 0, 0);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("shot");
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 500))
            {
                //Debug.Log(hit.transform.tag);
                //Debug.Log(hit.transform.position);
                //Debug.DrawRay(transform.localPosition, transform.TransformDirection(Vector3.forward) * 500, Color.red, 5f);
                if (hit.transform.tag == "aimlabs_fish")
                {
                    hit.transform.GetComponent<AL_fish_behavior>().loseHp();
                    Debug.Log("goonie fruitie");
                }
            }
        }
        
    }
}
