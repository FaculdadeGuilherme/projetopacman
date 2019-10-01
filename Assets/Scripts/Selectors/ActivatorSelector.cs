using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
using VRStandardAssets.Menu;

//Usada para ativar e desativar um objeto a partir do clique nele
public class ActivatorSelector : ClickSelector
{
    public GameObject target;
    [Tooltip("Use to dinamically find target")]
    public string targetName;
    [Tooltip("True to activate and false do deactivate target")]
    public bool willActivate;
    [Tooltip("True to enable undo")]
    public bool rollBack = false;
    bool timeToRollback = false;

    void Start()
    {
        if (target == null)
            target = GameObject.Find(targetName);
    }

    protected override void ActivateButton()
    {
        target.SetActive(rollBack ? (timeToRollback ? !willActivate : willActivate) :  willActivate);
        timeToRollback = !timeToRollback;
    }

    public void SetTargetName(string s)
    {
        targetName = s;
    }
}

