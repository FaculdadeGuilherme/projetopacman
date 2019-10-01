using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;
using VRStandardAssets.Menu;
using TMPro;

//Classe base para fazer scripts para detectar clique ou mouse over em objetos
public abstract class BaseSelector : MonoBehaviour
{
    protected ReferenceManager referenceManager;
    [Tooltip("Leave null if the component is already in this gameobject")]
    [SerializeField] protected VRInteractiveItem m_InteractiveItem;       // The interactive item for where the user should click to load the level.

    protected GameObject parent;

    protected void Awake()
    {
        if (m_InteractiveItem == null)
            m_InteractiveItem = GetComponent<VRInteractiveItem>();
        referenceManager = ReferenceManager.Instance;
    }

    public void SetParent(GameObject go)
    {
        parent = go;
    }

}

