using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class LittleGuyNav : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform target;

    [SerializeField] private float followDistance = 3f;
    [SerializeField] private float fleeDistance = 8f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        Player player = FindObjectOfType<Player>();

        if (player != null)
        {
            SetTarget(player.transform);
        }
        else
        {
            Debug.LogError("Player not found");
        }
    }

    public void HandleAI()
    {
        if (target == null || !navMeshAgent.isActiveAndEnabled) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget > followDistance)
        {
            FollowTarget();
        }
        //else if (distanceToTarget < fleeDistance)
        //{
        //    FleeFromTarget();
        //}
        else
        {
            navMeshAgent.isStopped = true;
        }
    }

    private void FollowTarget()
    {
        navMeshAgent.isStopped = false;

        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Vector3 stoppingPosition = target.position - directionToTarget * followDistance;
        navMeshAgent.SetDestination(stoppingPosition);
    }

    private void FleeFromTarget()
    {
        navMeshAgent.isStopped = false;
        Vector3 fleeDirection = (transform.position - target.position).normalized;
        Vector3 fleePosition = transform.position + fleeDirection * fleeDistance;
        navMeshAgent.SetDestination(fleePosition);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}