using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genshin_Result : MonoBehaviour
{
    public void SSR()
    {
        Minigame.Instance.InstantWin();
    }

    public void PoopGarbageR()
    {
        Minigame.Instance.InstantLose();
    }
}
