using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public List<string> lines;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetInteractionPrompt()
    {
        return "[E] Talk";
    }

    public void Interact() 
    {
        StartCoroutine(Dialog.Instance.StartDialog(lines));
    }
}
