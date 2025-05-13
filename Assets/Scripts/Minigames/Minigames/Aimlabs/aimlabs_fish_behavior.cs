using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AL_fish_behavior : MonoBehaviour
{
    Transform tr;
    private bool started;
    [SerializeField]
    private int hp;
    int randSize;
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        started = false;
        //Cursor.lockState = CursorLockMode.Locked;
        changePosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (!started)
        {
            if (Minigame.Instance.minigameState == MinigameState.Play)
            {
                hp = (int)(Minigame.Instance.minigameTime / 2) + Random.Range(-1, 1);
                if (hp <= 0)
                {
                    hp = 1;
                }
                started = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            loseHp();
        }
    }

    void changePosition()
    {
        randSize = Random.Range(5, 17);
        if (randSize % 2 == 1)
        {
            randSize *= -1;
        }
        tr.localPosition = new Vector3(Random.Range(-30, 30), Random.Range(8, 30), Random.Range(32, 36));
        tr.localScale = new Vector3(randSize * 2, Mathf.Abs(randSize), 1);

    }

    public void loseHp()
    {
        hp -= 1;
        if (hp <= 0)
        {
            Minigame.Instance.InstantWin();
        }
        else
        {
            changePosition();
        }
    }
}
