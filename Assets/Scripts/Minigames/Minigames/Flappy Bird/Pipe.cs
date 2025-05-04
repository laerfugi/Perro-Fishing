using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public float speed;
    private void FixedUpdate()
    {
        if (Minigame.Instance.minigameState == MinigameState.Play)
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x - speed * Time.fixedDeltaTime, gameObject.transform.position.y);
        }
    }
}
