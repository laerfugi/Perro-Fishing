using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPivot : MonoBehaviour
{
    [SerializeField] private Transform target;

    public void SetNewTarget(Transform newTarget)
    {
        target = newTarget;
        transform.position = target.position;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position;
        }
    }
}
