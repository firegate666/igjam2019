using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject NormalState;
    public GameObject ActiveState;
    
    public void ToggleState()
    {
        NormalState.SetActive(!NormalState.activeSelf);
        ActiveState.SetActive(!ActiveState.activeSelf);
    }

    public void TriggerAction()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
    }
}
