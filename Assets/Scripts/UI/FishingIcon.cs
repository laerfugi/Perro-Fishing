using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//cooldown fishing icon
public class FishingIcon : MonoBehaviour
{
    private FishingPole fishingPole;

    public Image icon;

    public float time;

    // Start is called before the first frame update
    void Start()
    {
        fishingPole = UIManager.Instance.player.GetComponentInChildren<FishingPole>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateIcon(fishingPole.elapsedCooldownTime/fishingPole.cooldownTime);
    }

    public void UpdateIcon(float t)
    {
        icon.fillAmount = t;

        time = t;

        if (t <1)
        {   
            Color color = icon.color;
            color.a = .5f;
            icon.color = color;
        }
        else
        {
            Color color = icon.color;
            color.a = 1f;
            icon.color = color;
        }
    }
}
