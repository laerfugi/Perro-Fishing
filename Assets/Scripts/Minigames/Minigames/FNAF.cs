using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FNAF : MonoBehaviour
{
    Animator jumpscareAnimator;
    bool started = false;
    // Start is called before the first frame update
    void Start()
    {
        jumpscareAnimator = GetComponent<Animator>();
        Minigame.Instance.Lose();
    }

    // Update is called once per frame
    void Update()
    {
        if (!started)
        {
            if (Minigame.Instance.minigameState == MinigameState.Play)
            {
                jumpscareAnimator.SetTrigger("start");
                started = true;
            }
        }
        else
        {
            if (Minigame.Instance.minigameTime == 0)
            {
                jumpscareAnimator.SetTrigger("lose");
            }
        }
    }

    public void FnafWin()
    {
        Minigame.Instance.InstantWin();
    }
}
