using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMplayerAnim : MonoBehaviour
{
    GameObject player;
    Animator anim;
    Animator redAnim;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        anim = player.GetComponentInChildren<Animator>();
        redAnim = GameObject.Find("Red Little Guy").GetComponentInChildren<Animator>();
        GameObject.Find("Green Little Guy").GetComponentInChildren<Animator>().SetBool("walk", true);
        GameObject.Find("Blue Little Guy").GetComponentInChildren<Animator>().SetBool("walk", true);
    }

    public void setWalk()
    {
        anim.SetBool("walk", true);
    }

    public void setLittleGuyWalk()
    {
        redAnim.SetBool("walk", true);
    }

    public void setIdle()
    {
        anim.SetBool("walk", false);
        anim.SetBool("run", false);
    }

    public void setLittleGuyIdle()
    {
        redAnim.SetBool("walk", false);
    }

    public void setRun()
    {
        anim.SetBool("walk", true);
        anim.SetBool("run", true);
    }
}
