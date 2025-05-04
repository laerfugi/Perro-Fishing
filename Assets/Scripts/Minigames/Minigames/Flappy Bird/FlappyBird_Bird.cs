using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyBird_Bird : MonoBehaviour
{
    Rigidbody2D rb;

    public float speed;
    public bool jump;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        Minigame.Instance.Win();
    }

    // Update is called once per frame
    void Update()
    {
        if ( Minigame.Instance.minigameState == MinigameState.Play)
        {
            if (Input.GetMouseButtonDown(0)) { jump = true; }
            rb.gravityScale = 1;
        }
        if (Minigame.Instance.minigameState == MinigameState.End)
        {
            rb.Sleep();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Minigame.Instance.InstantLose();
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (jump)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
            jump = false;
        }
    }
}
