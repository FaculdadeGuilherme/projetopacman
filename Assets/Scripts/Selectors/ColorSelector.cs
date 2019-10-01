using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
using VRStandardAssets.Menu;

//Usada para trocar a cor de um objeto, a partir do clique em um objeto
public class ColorSelector : ClickSelector
{

    public Color cor;
    public Material defaultMaterial;
    public MeshRenderer target;


    private void Start()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material.color = cor;
    }

    //QUANDO O BOTAO É ATIVADO, ESSA FUNCAO EH CHAMADA. AQUI VOCE DEVE PROGRAMAR O SEU CODIGO PARA O SEU BOTAO...
    protected override void ActivateButton()
    {
        target.material = defaultMaterial;
        target.material.color = cor;
    }
}

