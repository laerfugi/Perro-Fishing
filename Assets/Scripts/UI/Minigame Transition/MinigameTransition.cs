using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MinigameTransition : MonoBehaviour
{
    private Canvas canvas;

    public float transitionTime;

    [Header("Parent gameobject")]
    public GameObject transition;

    [Header("Panels")]
    public RectTransform leftPanel;
    public RectTransform rightPanel;
    public GameObject infoPanel;

    [Header("Progress Panel")]
    public GameObject progressPanel;
    public GameObject progressIcon;

    [Header("Minigame Manager Results")]
    public List<Result> results;

    void Start()
    {
        canvas = GetComponent<Canvas>();
        results = MinigameManager.Instance.results;

        transition.SetActive(false);
    }

    public IEnumerator Transition()
    {
        yield return StartCoroutine(CloseCurtains());
        yield return StartCoroutine(OpenCurtains());

    }

    public IEnumerator CloseCurtains()
    {
        transition.SetActive(true);
        leftPanel.gameObject.SetActive(true); rightPanel.gameObject.SetActive(true); infoPanel.SetActive(false);

        float elapsedTime = 0;
        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;

            //need to change this
            leftPanel.position = Vector3.Lerp(new Vector3(-canvas.pixelRect.size.x * 1f, leftPanel.position.y), new Vector3(canvas.pixelRect.size.x * .25f, leftPanel.position.y), elapsedTime / transitionTime);
            rightPanel.position = Vector3.Lerp(new Vector3(canvas.pixelRect.size.x * 2f, leftPanel.position.y), new Vector3(canvas.pixelRect.size.x * .75f, leftPanel.position.y), elapsedTime / transitionTime);

            yield return null;
        }

        yield return StartCoroutine(ShowProgress());
    }

    IEnumerator ShowProgress()
    {
        //Intro Delay
        yield return new WaitForSeconds(.1f);
        infoPanel.SetActive(true);

        //Create a row of progress icons representing minigame results
        List<GameObject> progressIconList = new List<GameObject>();
        for (int i = 0; i < MinigameManager.Instance.results.Count; i++)
        {
            GameObject newProgressIcon = Instantiate(progressIcon, progressPanel.transform);
            progressIconList.Add(newProgressIcon);

            //win/lose icons
            if (MinigameManager.Instance.results[i] == Result.Win)
            {
                newProgressIcon.GetComponent<Image>().color = Color.green;
            }
            else if (MinigameManager.Instance.results[i] == Result.Lose)
            {
                newProgressIcon.GetComponent<Image>().color = Color.red;
            }
        }

        //Outro Delay
        yield return new WaitForSeconds(2f);
        infoPanel.SetActive(false);

        //delete all icons, this is so bad but it works
        for (int i = 0; i < progressIconList.Count; i++)
        {
            Destroy(progressIconList[i]);
        }
    }

    public IEnumerator OpenCurtains()
    {
        yield return new WaitForSeconds(.1f);

        float elapsedTime = 0;
        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;

            //need to change this
            leftPanel.position = Vector3.Lerp(new Vector3(canvas.pixelRect.size.x * .25f, leftPanel.position.y), new Vector3(-canvas.pixelRect.size.x * 1f, leftPanel.position.y), elapsedTime / transitionTime);
            rightPanel.position = Vector3.Lerp(new Vector3(canvas.pixelRect.size.x * .75f, leftPanel.position.y), new Vector3(canvas.pixelRect.size.x * 2f, leftPanel.position.y), elapsedTime / transitionTime);
            yield return null;
        }

        transition.SetActive(false);
        leftPanel.gameObject.SetActive(false); rightPanel.gameObject.SetActive(false);
    }
}
