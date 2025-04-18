using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FishingState {Inactive,Casting,Catching,Cooldown}    //casting: Little Guy is being thrown, Catching: Little Guy touched the ground and is looking for fish

public class FishingPole : MonoBehaviour
{
    public GameObject littleGuy;

    [Header("Locations")]
    public GameObject startLocation;
    public GameObject targetLocation;

    [Header("Fishing Variables")]
    private float elapsedTime;
    public float castTime;  //how long it takes to cast the bait
    public float cooldownTime;

    public FishingState state;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //start fishing
        if (GameObject.FindWithTag("Player").GetComponent<Player>().state == PlayerState.Active)
        {
            if (state == FishingState.Inactive)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(Fishing());
                }
            }
        }
        //what happens during Catching
        else if (state == FishingState.Catching)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(Cooldown());
            }
        }
    }

    IEnumerator Fishing()
    {
        //set up 
        elapsedTime = 0;
        state = FishingState.Casting;

        GameObject.FindWithTag("Player").GetComponent<Player>().state = PlayerState.Inactive;
        littleGuy.GetComponent<LittleGuy>().state = LittleGuyState.Inactive;

        littleGuy.transform.position = startLocation.transform.position;

        //choose targetLocation

        //cast LittleGuy to targetLocation
        while (elapsedTime < castTime)
        {
            littleGuy.transform.position = Vector3.Slerp(startLocation.transform.position, targetLocation.transform.position, elapsedTime / castTime);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        yield return new WaitUntil(() => elapsedTime > castTime);

        //post cast states
        littleGuy.GetComponent<LittleGuy>().state = LittleGuyState.Active;

        yield return new WaitForSeconds(2f);
        state = FishingState.Catching;
    }

    IEnumerator Cooldown()
    {
        state = FishingState.Cooldown;
        GameObject.FindWithTag("Player").GetComponent<Player>().state = PlayerState.Active;
        littleGuy.GetComponent<LittleGuy>().state = LittleGuyState.AI;
        yield return new WaitForSeconds(cooldownTime);
        state = FishingState.Inactive;
    }
}
