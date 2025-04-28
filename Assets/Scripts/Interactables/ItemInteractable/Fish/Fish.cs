using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : ItemInteractable
{
    private void OnEnable()
    {
        EventManager.EndMinigameEvent += ProcessMinigameResult;
    }
    private void OnDisable()
    {
        EventManager.EndMinigameEvent -= ProcessMinigameResult;   
    }

    //instead of adding fish to inventory, initiates a minigame
    public override void Interact()
    {
        Debug.Log("start minigame");
        MinigameManager.Instance.StartMinigame("minigamename");
    }

    public override void Use()
    {
        Debug.Log("I am a fish");
    }

    void ProcessMinigameResult(bool result)
    {
        if (result == true) { Debug.Log("you won! i should despawn and go to inventory"); }
        else if (result == false) { Debug.Log("you lost... i should despawn and run away"); }
    }
}