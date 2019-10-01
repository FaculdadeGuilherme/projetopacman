using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
using VRStandardAssets.Menu;

//Usada para ativar e desativar a parte visual de um modelo 3d, a partir do clique em um objeto
public class MeshRendererSelector : ClickSelector
{
    public MeshRenderer target;
    [Tooltip("Use to dinamically find target")]
    public string targetName;
    [Tooltip("True to activate and false do deactivate target")]
    public bool willActivate;
    [Tooltip("True to enable undo")]
    public bool rollBack = false;
    bool timeToRollback = false;

    void Start()
    {
        //if (target == null)
         //   target = GameObject.Find(targetName).GetComponent<typeof(target)>();
    }

    protected override void ActivateButton()
    {        
        target.enabled = (rollBack ? (timeToRollback ? !willActivate : willActivate) :  willActivate);
        timeToRollback = !timeToRollback;
    }

    public void SetTargetName(string s)
    {
        targetName = s;
    }
}

