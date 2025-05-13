using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MMplayerAnim : MonoBehaviour
{
    GameObject player;
    Animator anim;
    Animator redAnim;
    public Database db;
    SpriteRenderer lgSprite, fishSprite;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("mainMenuPlayer");
        anim = player.GetComponentInChildren<Animator>();
        redAnim = GameObject.Find("Red Little Guy").GetComponentInChildren<Animator>();
        lgSprite = GameObject.Find("Red Little Guy").GetComponentInChildren<SpriteRenderer>();
        fishSprite = GameObject.Find("mainMenuGoldfish").GetComponent<SpriteRenderer>();
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

    public void setSprites()
    {
        lgSprite.sprite = db.littleGuyList[Random.Range(0, db.littleGuyList.Count)].icon;
        fishSprite.sprite = db.fishList[Random.Range(0, db.fishList.Count)].icon;
    }
}
