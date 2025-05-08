using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum FishingState {Inactive,Casting,Catching,Cooldown}    //Inactive: You can press Mouse1 to Cast
                                                                 //Casting: Little Guy is being thrown
                                                                 //Catching: Little Guy touched the ground and is looking for fish
                                                                 //Cooldown: Wait until starte becomes Inactive

public class FishingPole : MonoBehaviour
{
    public Transform cameraPivot;
    public LittleGuy_ItemDataWrapper littleGuy_ItemDataWrapper;
    private GameObject littleGuy;

    [Header("Locations")]
    public GameObject startLocation;
    public GameObject targetLocation;

    [Header("Fishing Variables")]
    public float castTime;  //how long it takes to cast the bait
    public float elapsedCastTime { get; private set; }

    public float cooldownTime;
    public float elapsedCooldownTime { get; private set; }

    public FishingState state;

    [Header("Path Indicator")]
    public LineRenderer lineRenderer;
    public int pathResolution = 20;

    // Start is called before the first frame update
    void Start()
    {
        //this is so bad
        if (PlayerInventory.Instance.littleGuyInventoryList.Count > 1) {
            littleGuy_ItemDataWrapper = PlayerInventory.Instance.littleGuyInventoryList[0];
        }

        elapsedCooldownTime = cooldownTime;
        targetLocation.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerInventory.Instance.littleGuyInventoryList.Count > 1)
        {
            littleGuy_ItemDataWrapper = PlayerInventory.Instance.littleGuyInventoryList[0];
        }

        if (!UIManager.Instance.menuIsOpen)     //need to change how to disable inputs 
        {
            //start fishing
            if (GameObject.FindWithTag("Player").GetComponent<Player>().state == PlayerState.Active)
            {
                if (state == FishingState.Inactive)
                {
                    if (Input.GetMouseButton(0))
                    {
                        FacePlayerToCamera();
                        ShowPath();
                    }

                    if (Input.GetMouseButtonUp(0))
                    {
                        ClearPath();
                        StartCoroutine(Fishing());
                    }

                }
            }
            //what happens during Catching
            else if (state == FishingState.Catching)
            {
                UnfacePlayerToCamera();

                if (Input.GetMouseButtonDown(1))
                {
                    StartCoroutine(Cooldown());
                }
            }
        }
    }

    IEnumerator Fishing()
    {
        if (littleGuy_ItemDataWrapper == null) { Debug.Log("need to equip little guy!"); yield break; }

        littleGuy = littleGuy_ItemDataWrapper.littleGuy;

        //set up 
        elapsedCastTime = 0;
        state = FishingState.Casting;

        elapsedCooldownTime = 0;

        GameObject.FindWithTag("Player").GetComponent<Player>().ChangeState(PlayerState.Fishing);
        littleGuy.GetComponent<LittleGuy>().ChangeState(LittleGuyState.Inactive);

        littleGuy.transform.position = startLocation.transform.position;

        Vector3 previousPosition = startLocation.transform.position;

        //cast LittleGuy to targetLocation
        while (elapsedCastTime < castTime)
        {
            float t = elapsedCastTime / castTime;
            Vector3 nextPosition = CalculateParabolaPosition(t, startLocation.transform.position, targetLocation.transform.position);

            // Check for collisions between the previous position and the next position
            if (Physics.Linecast(previousPosition, nextPosition, out RaycastHit hit))
            {
                Debug.Log($"Collision detected at {hit.point}");
                littleGuy.transform.position = previousPosition; // Place little guy before hit
                elapsedCastTime += 1; // Forcibly break
                break;
            }

            littleGuy.transform.position = nextPosition;
            previousPosition = nextPosition;

            elapsedCastTime += Time.deltaTime;

            yield return null;
        }

        //post cast

        littleGuy.GetComponent<LittleGuy>().ChangeState(LittleGuyState.Active);
        state = FishingState.Catching;
    }

    IEnumerator Cooldown()
    {
        state = FishingState.Cooldown;
        GameObject.FindWithTag("Player").GetComponent<Player>().ChangeState(PlayerState.Active);
        littleGuy.GetComponent<LittleGuy>().ChangeState(LittleGuyState.AI);
        //yield return new WaitForSeconds(cooldownTime);
        elapsedCooldownTime = 0;
        while (elapsedCooldownTime < cooldownTime) 
        { 
            elapsedCooldownTime += Time.deltaTime;
            yield return null;
        }

        
        state = FishingState.Inactive;
    }
    void ShowPath()
    {
        //line renderer
        if (state != FishingState.Inactive) return;

        Vector3[] pathPoints = CalculatePathPoints();

        lineRenderer.positionCount = pathPoints.Length;
        lineRenderer.SetPositions(pathPoints);

        //marker
        targetLocation.SetActive(true);
    }

    void ClearPath()
    {
        //line renderer
        lineRenderer.positionCount = 0;

        //marker
        targetLocation.SetActive(false);
    }

    //Vector3[] CalculatePathPoints()
    //{
    //    Vector3[] points = new Vector3[pathResolution];
    //    for (int i = 0; i < pathResolution; i++)
    //    {
    //        float t = (float)i / (pathResolution - 1); // Normalize t between 0 and 1
    //        points[i] = Vector3.Slerp(startLocation.transform.position, targetLocation.transform.position, t);
    //    }
    //    return points;
    //}

    Vector3[] CalculatePathPoints()
    {
        Vector3[] points = new Vector3[pathResolution];
        Vector3 start = startLocation.transform.position;
        Vector3 target = targetLocation.transform.position;

        for (int i = 0; i < pathResolution; i++)
        {
            float t = (float)i / (pathResolution - 1); // Normalize t between 0 and 1
            points[i] = CalculateParabolaPosition(t, start, target);
        }

        return points;
    }

    Vector3 CalculateParabolaPosition(float t, Vector3 start, Vector3 target)
    {
        float apexHeight = Mathf.Max(start.y, target.y) + 4f; // Arc apex height
        Vector3 horizontalPosition = Vector3.Lerp(start, target, t);

        // Interpolate between start to peak to target height
        float height = Mathf.Lerp(start.y, apexHeight, t) * (1 - t) + Mathf.Lerp(apexHeight, target.y, t) * t;

        return new Vector3(horizontalPosition.x, height, horizontalPosition.z);
    }

    void FacePlayerToCamera()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerMovementHandler>().fishing = true;
    }

    void UnfacePlayerToCamera()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerMovementHandler>().fishing = false;
    }
}
