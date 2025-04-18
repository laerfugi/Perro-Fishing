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

    [Header("Path Indicator")]
    public LineRenderer lineRenderer;
    public int pathResolution = 20;

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

        Vector3 previousPosition = startLocation.transform.position;

        //choose targetLocation

        //cast LittleGuy to targetLocation
        while (elapsedTime < castTime)
        {
            float t = elapsedTime / castTime;
            Vector3 nextPosition = CalculateParabolaPosition(t, startLocation.transform.position, targetLocation.transform.position);

            // Check for collisions between the previous position and the next position
            if (Physics.Linecast(previousPosition, nextPosition, out RaycastHit hit))
            {
                Debug.Log($"Collision detected at {hit.point}");
                littleGuy.transform.position = previousPosition; // Place little guy before hit
                elapsedTime += 1; // Forcibly break
                break;
            }

            littleGuy.transform.position = nextPosition;
            previousPosition = nextPosition;

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
    void ShowPath()
    {
        if (state != FishingState.Inactive) return;

        Vector3[] pathPoints = CalculatePathPoints();

        lineRenderer.positionCount = pathPoints.Length;
        lineRenderer.SetPositions(pathPoints);
    }

    void ClearPath()
    {
        lineRenderer.positionCount = 0;
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
        Transform playerTransform = GameObject.FindWithTag("Player").transform;

        Vector3 cameraForward = Camera.main.transform.forward;

        cameraForward.y = 0;
        cameraForward.Normalize();

        playerTransform.rotation = Quaternion.LookRotation(cameraForward);
    }
}
