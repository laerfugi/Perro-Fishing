using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinigameTransition : MonoBehaviour
{
    private Canvas canvas;

    public float transitionTime;

    [Header("Panels")]
    public RectTransform leftPanel;
    public RectTransform rightPanel;
    public GameObject infoPanel;

    void Awake()
    {
        canvas = GetComponent<Canvas>();

        //StartCoroutine(Transition());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Transition()
    {
        yield return StartCoroutine(CloseCurtains());
        yield return StartCoroutine(OpenCurtains());

    }

    public IEnumerator CloseCurtains()
    {
        leftPanel.gameObject.SetActive(true); rightPanel.gameObject.SetActive(true);

        float elapsedTime = 0;
        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;

            //need to change this
            leftPanel.position = Vector3.Lerp(new Vector3(-canvas.pixelRect.size.x * .25f, leftPanel.position.y), new Vector3(canvas.pixelRect.size.x * .25f, leftPanel.position.y), elapsedTime / transitionTime);
            rightPanel.position = Vector3.Lerp(new Vector3(canvas.pixelRect.size.x * 1.25f, leftPanel.position.y), new Vector3(canvas.pixelRect.size.x * .75f, leftPanel.position.y), elapsedTime / transitionTime);

            yield return null;
        }

        yield return StartCoroutine(ShowInfo());
    }

    IEnumerator ShowInfo()
    {
        yield return new WaitForSeconds(.1f);
        infoPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        infoPanel.SetActive(false);
    }

    public IEnumerator OpenCurtains()
    {
        yield return new WaitForSeconds(.1f);

        float elapsedTime = 0;
        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;

            //need to change this
            leftPanel.position = Vector3.Lerp(new Vector3(canvas.pixelRect.size.x * .25f, leftPanel.position.y), new Vector3(-canvas.pixelRect.size.x * .25f, leftPanel.position.y), elapsedTime / transitionTime);
            rightPanel.position = Vector3.Lerp(new Vector3(canvas.pixelRect.size.x * .75f, leftPanel.position.y), new Vector3(canvas.pixelRect.size.x * 1.25f, leftPanel.position.y), elapsedTime / transitionTime);
            yield return null;
        }

        leftPanel.gameObject.SetActive(false); rightPanel.gameObject.SetActive(false);
    }
}
