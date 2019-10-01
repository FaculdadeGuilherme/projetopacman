using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
using VRStandardAssets.Menu;

//Usada para telerportar um objeto a partir do clique em um objeto
public class TeleportObjectorSelector : ClickSelector
{
    public GameObject target;
    [Tooltip("Use to dinamically find target")]
    public string targetName;

    public Vector3 teleportDestiny;
    Vector3 teleportOrigin;
    
    void Start()
    {
        if (target == null)
            target = GameObject.Find(targetName);
        teleportOrigin = target.transform.position;
    }

    protected override void ActivateButton()
    {
        if (Vector3.Distance(target.transform.position, teleportOrigin) < 1)
            target.transform.position = teleportDestiny;
        else
            target.transform.position = teleportOrigin;
    }

    public void SetTargetName(string s)
    {
        targetName = s;
    }
}

