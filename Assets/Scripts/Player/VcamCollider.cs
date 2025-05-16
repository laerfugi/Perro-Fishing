using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VcamCollider : MonoBehaviour
{
    public GameObject model;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Camera")) { model.SetActive(false); }
    }

    private void OnTriggerExit(Collider other)
    {
        model.SetActive(true);
    }
}
