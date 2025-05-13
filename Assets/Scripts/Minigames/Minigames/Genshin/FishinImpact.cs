using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishenImpact : MonoBehaviour
{
    public GameObject pullAnimation;
    public int winrate;
    private Animator fishPull;
    // Start is called before the first frame update
    void Start()
    {
        fishPull = pullAnimation.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SinglePull()
    {
        if (Random.Range(1, 100) <= winrate)
        {
            fishPull.SetTrigger("win");
            gameObject.SetActive(false);
        } 
        else
        {
            fishPull.SetTrigger("lose");
            gameObject.SetActive(false);
        }
    }
}
