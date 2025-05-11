using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Fish : MonoBehaviour, IInteractable
{
    [Header("Item Data")]
    public Fish_ItemData itemData;

    [Header("Info")]
    public Database database;
    public Lake lake;

    [Header("Animator")]
    public Animator animator;

    [Header("Debug")]
    public bool skipMinigame;

    private void Awake()
    {

    }

    private void OnDestroy()
    {
        if (lake) { lake.fishList.Remove(this.gameObject); }
    }

    void Update()
    {

    }

    //instead of adding fish to inventory, initiates a minigame
    public void Interact()
    {
        if (skipMinigame) 
        {
            List<Result> temp = new List<Result>();
            temp.Add(Result.Win);
            ProcessMinigameResults(temp);
        }
        else 
        {
            StartCoroutine(Minigame());
        }
    }

    public void Use()
    {
        Debug.Log("I am a fish");
    }
    public string GetInteractionPrompt()
    {
        return $"[E] {itemData.name}";
    }

    IEnumerator Minigame()
    {
        this.gameObject.layer = 0;
        yield return MinigameManager.Instance.StartCoroutine(MinigameManager.Instance.LaunchMinigames(1));
        ProcessMinigameResults(MinigameManager.Instance.results);
    }

    void ProcessMinigameResults(List<Result> results)
    {
        if (results.Contains(Result.Lose)) 
        {
            Debug.Log("you lost... i should despawn and run away");
            StartCoroutine(Animate());

        }
        else 
        {
            Debug.Log("you won! i should despawn and go to inventory");
            //choose random fish from list
            itemData = database.fishList[Random.Range(0, database.fishList.Count)];

            PlayerInventory.Instance.AddItem(itemData);
            StartCoroutine(Animate());
        }
    }

    IEnumerator Animate()
    {
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.transform.rotation = Quaternion.Euler(0, 0, 90);   //need to change this 
        animator.SetBool("Catch", true);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Animation finished"));
        Destroy(gameObject);
    }


}