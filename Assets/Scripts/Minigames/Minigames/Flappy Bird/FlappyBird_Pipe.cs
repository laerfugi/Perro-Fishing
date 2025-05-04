using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyBird_Pipe : MonoBehaviour
{
    public float speed;
    private void Update()
    {
        if (Minigame.Instance.minigameState == MinigameState.Play)
        {
            gameObject.transform.position = transform.position + Vector3.left * speed * Time.deltaTime;
        }
    }
}
