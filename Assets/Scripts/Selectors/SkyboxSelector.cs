using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
using VRStandardAssets.Menu;

//Usada para trocar de skybox a partir do clique em um objeto
public class SkyboxSelector : ClickSelector
{
    public Material newSkybox;
    Material oldSkybox;
    bool isOn = false;

    void Start()
    {
        oldSkybox = RenderSettings.skybox;
    }

    protected override void ActivateButton()
    {
        isOn = !isOn;
        RenderSettings.skybox = isOn ? newSkybox : oldSkybox;
    }

    public void SetTargetName(string s)
    {
        
    }
}

