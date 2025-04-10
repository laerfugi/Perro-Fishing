using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    // Player can use/interact with an item
    public static event UnityAction PlayerCanInteractEvent;
    public static event UnityAction PlayerCannotInteractEvent;

    public static void OnPlayerCanInteractEvent() => PlayerCanInteractEvent?.Invoke();
    public static void OnPlayerCannotInteractEvent() => PlayerCannotInteractEvent?.Invoke();
}