using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;
using VRStandardAssets.Menu;
using TMPro;

//Classe abstrata para detectar cliques em objetos
public abstract class ClickSelector : BaseSelector
{
    protected void OnEnable()
    {
        m_InteractiveItem.OnClick += ActivateButton;
        
    }
    
    protected void OnDisable()
    {
        
        m_InteractiveItem.OnClick -= ActivateButton;
    }


    //QUANDO O BOTAO É ATIVADO, ESSA FUNCAO EH CHAMADA. AQUI VOCE DEVE PROGRAMAR O SEU CODIGO PARA O SEU BOTAO...
    protected abstract void ActivateButton();
}

