using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class LittleGuyThoughts : MonoBehaviour
{
    public LittleGuy littleGuy;

    [Header("Output")]
    public TMP_Text text;
    public SpriteRenderer spriteRenderer;

    [Header("Faces")]
    public List<string> faces;
    public List<Sprite> spriteFaces;
    public Sprite exclamation;

    [Header("Settings")]
    public float fadeOutTime;
    private bool inAnimation;

    // Start is called before the first frame update
    void Start()
    {
        //text.gameObject.SetActive(true);
        spriteRenderer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        Think();
    }

    public IEnumerator AnimateText(string thought)
    {
        if (!inAnimation)
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
    }

    public IEnumerator AnimateSprite(Sprite thought)
    {
        if (!inAnimation)
        {
            inAnimation = true;
            spriteRenderer.enabled = true;

            spriteRenderer.sprite = thought;

            yield return new WaitForSeconds(1f);

            Color temp = spriteRenderer.color;

            //fade out
            float timeElapsed = 0;
            while (timeElapsed < fadeOutTime)
            {
                temp.a = (fadeOutTime - timeElapsed) / fadeOutTime;
                spriteRenderer.color = temp;
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            temp.a = 0;
            spriteRenderer.color = temp;

            inAnimation = false;
            spriteRenderer.enabled = false;
            
            //reset alpha
            temp.a = 1;
            spriteRenderer.color = temp;
        }
    }

    public void RandomAnimateText()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateText(faces[Random.Range(0,faces.Count)]));
    }

    public void RandomAnimateSprite()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateSprite(spriteFaces[Random.Range(0,spriteFaces.Count)]));
    }

    void Think()
    {
        if (!inAnimation)
        {
            if (!littleGuy.isCaught)
            {
                //text.gameObject.SetActive(true);
                //text.text = "?";

                spriteRenderer.enabled = true;
                spriteRenderer.sprite = exclamation;     //need to change
            }
            else
            {
                //text.gameObject.SetActive(false);
                spriteRenderer.enabled = false;
            }
        }
    }
}
