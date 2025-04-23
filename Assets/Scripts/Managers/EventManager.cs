using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    #region Events

    /*---Player---*/
    #region Player
    // Player inventory is updated
    public static event UnityAction InventoryEvent;

    // Player can use/interact with an item
    public static event UnityAction<string> PlayerCanInteractEvent;
    public static event UnityAction PlayerCannotInteractEvent;

    //Player State has changed
    public static event UnityAction<PlayerState> PlayerStateEvent;
    #endregion


    //Little Guy State has changed
    public static event UnityAction<LittleGuyState> LittleGuyStateEvent;

    //Switch VCam (used as a method)
    public static event UnityAction<GameObject> SwitchVCamEvent;

    //A menu has been toggled
    public static event UnityAction MenuEvent;


    #endregion

    #region Invoke Methods
    public static void OnInventoryEvent() => InventoryEvent?.Invoke();
    public static void OnPlayerCanInteractEvent(string message) => PlayerCanInteractEvent?.Invoke(message);
    public static void OnPlayerCannotInteractEvent() => PlayerCannotInteractEvent?.Invoke();
    public static void OnPlayerStateEvent(PlayerState playerState) => PlayerStateEvent?.Invoke(playerState);
    public static void OnLittleGuyStateEvent(LittleGuyState littleGuyState) => LittleGuyStateEvent?.Invoke(littleGuyState);
    public static void OnSwitchVCamEvent(GameObject vcam) => SwitchVCamEvent?.Invoke(vcam);
    public static void OnMenuEvent() => MenuEvent?.Invoke();
    #endregion
}