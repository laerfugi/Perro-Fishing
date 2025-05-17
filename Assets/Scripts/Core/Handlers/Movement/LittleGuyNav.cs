using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class LittleGuyNav : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;       //i made this public for LittleGuyAnimationHandler to read it
    private AudioSource fleeNoise;
    [SerializeField] private Transform target;

    [SerializeField] private float followDistance = 3f;
    [SerializeField] private float fleeDistance = 8f;

    public bool isFleeing = true;
    [SerializeField]
    private bool isUncatchable = false;

    [SerializeField]
    private Action onInitialTargetReached;
    [SerializeField]
    private Vector3? initialRunTarget = null;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        fleeNoise = GetComponent<AudioSource>();
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
        // Bring little guy to flee to random position before allowing it to be captured
        if (initialRunTarget.HasValue)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(initialRunTarget.Value);

            if (Vector3.Distance(transform.position, initialRunTarget.Value) < 1f)
            {
                initialRunTarget = null;
                onInitialTargetReached?.Invoke();
                onInitialTargetReached = null;
            }
            return;
        }

        if (target == null || !navMeshAgent.isActiveAndEnabled) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (isFleeing)
        {
            /*if (!fleeNoise.isPlaying)
            {
                fleeNoise.Play();
            }*/
            HandleFleeing(distanceToTarget);
        }
        else if (distanceToTarget > followDistance)
        {
            fleeNoise.Stop();
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

    public void RunToInitialTarget(Vector3 target, Action onReached)
    {
        AudioManager.Instance.AddAudioSource(fleeNoise);
        fleeNoise.volume = AudioManager.Instance.soundVolume;
        fleeNoise.Play();
        initialRunTarget = target;
        onInitialTargetReached = onReached;
    }

    public void SetSpeed(float speed, float acceleration)
    {
        navMeshAgent.speed = speed;
        navMeshAgent.acceleration = acceleration;
    }

    public void SetUncatchable(bool uncatchable)
    {
        isUncatchable = uncatchable;
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

    private void HandleFleeing(float distanceToTarget)
    {
        if (distanceToTarget > fleeDistance)
        {
            return;
        }
        else if (distanceToTarget < followDistance && !isUncatchable) { // if caught

            isFleeing = false;
            navMeshAgent.speed = 8f;
            navMeshAgent.acceleration = 8f;
            return;
        }

        navMeshAgent.isStopped = false;

        navMeshAgent.speed = 4f;
        navMeshAgent.acceleration = 4f;

        Vector3 fleeDirection = (transform.position - target.position).normalized;
        Vector3 fleePosition = transform.position + fleeDirection * fleeDistance;
        navMeshAgent.SetDestination(fleePosition);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}