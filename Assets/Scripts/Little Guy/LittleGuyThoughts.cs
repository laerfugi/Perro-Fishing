using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class LittleGuyThoughts : MonoBehaviour
{
    public LittleGuy littleGuy;

    public TMP_Text text;

    public List<string> faces;

    public float fadeOutTime;
    private bool inAnimation;

    // Start is called before the first frame update
    void Start()
    {
        text.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        Think();
    }

    public IEnumerator Animate(string thought)
    {
        inAnimation = true;
        text.gameObject.SetActive(true);
        text.text = thought;
        yield return new WaitForSeconds(2f);

        //fade out
        float timeElapsed = 0;
        while (timeElapsed < fadeOutTime)
        {
            text.alpha = (fadeOutTime - timeElapsed) / fadeOutTime;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        text.alpha = 0;
        text.text = "";

        inAnimation = false;
        text.gameObject.SetActive(false);
    }

    public IEnumerator RandomAnimate()
    {
        yield return StartCoroutine(Animate(faces[Random.Range(0,faces.Count)]));
    }

        void Think()
    {
        if (!inAnimation)
        {
            if (!littleGuy.isCaught)
            {
                text.gameObject.SetActive(true);
                text.text = "?";
            }
            else
            {
                text.gameObject.SetActive(false);
            }
        }
    }
}
