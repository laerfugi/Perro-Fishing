using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMplayerAnim : MonoBehaviour
{
    GameObject player;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        anim = player.GetComponentInChildren<Animator>();
    }

    public void setWalk()
    {
        anim.SetBool("walk", true);
    }

    public void setIdle()
    {
        anim.SetBool("walk", false);
        anim.SetBool("run", false);
    }

    public void setRun()
    {
        anim.SetBool("walk", true);
        anim.SetBool("run", true);
    }
}
