using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;

/// <summary>Usada para rotacionar um objeto da GUI quando o mouse esta mirando nele
/// , atualmente em um unico eixo</summary>
public class SpinUIImage : MonoBehaviour {

    public float angularSpeed = 5f;
    public LerpEquationTypes lerp;
    public float initialZ = 0, finalZ = -360;
    float currentSpeed = 0;
    float timeCounter;
    [SerializeField] protected VRInteractiveItem m_InteractiveItem;       // The interactive item for where the user should click to load the level.
    RectTransform myRect;

    // Use this for initialization
    void Awake () {
        if(m_InteractiveItem == null)
           m_InteractiveItem = GetComponent<VRInteractiveItem>();
        myRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        timeCounter += Time.deltaTime * currentSpeed;
        myRect.localRotation = Quaternion.Euler(0,0,lerp.Lerp(initialZ, finalZ, timeCounter));// Rotate(0, 0, currentSpeed * Time.deltaTime, Space.Self);
        if (timeCounter > 1)
            timeCounter = 0;
    }


    private void OnEnable()
    {
        m_InteractiveItem.OnOver += HandleOver;
        m_InteractiveItem.OnOut += HandleOut;
    }


    private void OnDisable()
    {
        m_InteractiveItem.OnOver -= HandleOver;
        m_InteractiveItem.OnOut -= HandleOut;
    }

    private void HandleOut()
    {
        // When the user looks away from the rendering of the scene, hide the radial.
        currentSpeed = 0;
    }

    private void HandleOver()
    {
        currentSpeed = angularSpeed;
    }  
}
