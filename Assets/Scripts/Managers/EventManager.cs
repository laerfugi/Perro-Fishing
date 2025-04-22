using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    #region Events
    // Player inventory is updated
    public static event UnityAction InventoryEvent;

    // Player can use/interact with an item
    public static event UnityAction<string> PlayerCanInteractEvent;
    public static event UnityAction PlayerCannotInteractEvent;

    /*---Camera Manager Events---*/
    public static event UnityAction<GameObject> SwitchVCamEvent;

    /*---UI Events---*/
    #region UI Events
    #endregion

    #endregion

    #region Invoke Methods
    public static void OnInventoryEvent() => InventoryEvent?.Invoke();
    public static void OnPlayerCanInteractEvent(string message) => PlayerCanInteractEvent?.Invoke(message);
    public static void OnPlayerCannotInteractEvent() => PlayerCannotInteractEvent?.Invoke();
    public static void OnSwitchVCamEvent(GameObject vcam) => SwitchVCamEvent?.Invoke(vcam);
    #endregion
}