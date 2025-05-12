using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderingContainer : MonoBehaviour
{
    [Header("Wandering Settings")]
    public float baseWaitTime = 2f;

    [Header("Dependencies")]
    public Transform wanderArea;

    private NavMeshAgent navMeshAgent;
    private Vector3 targetPosition;
    private bool isWandering = true;

    public bool isPlayerInteracting = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        StartCoroutine("Wander");
    }

    void Update()
    {
        if (isPlayerInteracting)
        {
            navMeshAgent.isStopped = true;
        }
        // implement this and a flag for Wander() if planned to extend to real NPCs
        //else if (!navMeshAgent.isStopped && isWandering && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        //{
        //    // If the agent has reached its destination, start wandering again
        //    //StartCoroutine(Wander());
        //}
    }

    private IEnumerator Wander()
    {
        //Debug.Log("called Wander()");
        while (isWandering && !isPlayerInteracting)
        {
            targetPosition = GetRandomPositionWithinBounds();
            navMeshAgent.SetDestination(targetPosition);

            while (navMeshAgent.pathPending)
            {
                yield return null;
            }

            // Wait for a random time before setting the next destination
            float waitTime = baseWaitTime + Random.Range(0f, 2f);
            //Debug.Log($"waiting for {waitTime}");
            yield return new WaitForSeconds(waitTime);
        }
    }

    private Vector3 GetRandomPositionWithinBounds()
    {
        if (wanderArea == null)
        {
            Debug.LogError("Wander area is not assigned!");
            return transform.position;
        }

        // Get the bounds of the wander area
        Bounds bounds = new Bounds(wanderArea.position, wanderArea.localScale);

        Vector3 randomPosition = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            transform.position.y,  // Assuming water/terrain is flat
            Random.Range(bounds.min.z, bounds.max.z)
        );
        
        return randomPosition;
    }

    public void StopWandering()
    {
        //Debug.Log("Stop wandering has been called!!");
        StopCoroutine("Wander");
        isWandering = false;
        navMeshAgent.isStopped = true;
    }

    public void ResumeWandering()
    {
        isWandering = true;
        navMeshAgent.isStopped = false;
    }
}