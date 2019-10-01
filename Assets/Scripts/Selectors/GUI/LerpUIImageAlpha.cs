using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;


/// <summary>Usada para apagar um objeto da GUI, mexendo no seu alpha,
/// quando o mouse esta em cima dele.</summary>
[RequireComponent(typeof(VRInteractiveItem))]
public class LerpUIImageAlpha : MonoBehaviour {

    public float transitionInDuration = 0.5f, transitionOutDuration = 0.5f;
    public LerpEquationTypes lerp;

    float lerpFactor, lerpA, lerpB;
    float totalLerpDuration;
    Image myImageComponent;
    protected float countDownToTurnOff;
    Color myColor = new Color(0, 0, 0, 0);

    [SerializeField] protected VRInteractiveItem m_InteractiveItem;       // The interactive item for where the user should click to load the level.

    // Use this for initialization
    void Awake () {
        myImageComponent = GetComponent<Image>();
        myColor = myImageComponent.color;
        if (m_InteractiveItem == null)
            m_InteractiveItem = GetComponent<VRInteractiveItem>();
        ResetButtonVisual();
    }

    void Update()
    {
        countDownToTurnOff -= Time.deltaTime;
        if (countDownToTurnOff > 0)
        {
            lerpFactor = countDownToTurnOff / totalLerpDuration;
            myColor = myImageComponent.color;
            myColor.a = lerp.Lerp(lerpA, lerpB, 1 - lerpFactor);
            myImageComponent.color = myColor;
        }
    }


    private void OnEnable()
    {
        m_InteractiveItem.OnOver += HandleOver;
        m_InteractiveItem.OnOut += HandleOut;
        ResetButtonVisual();
    }


    private void OnDisable()
    {
        m_InteractiveItem.OnOver -= HandleOver;
        m_InteractiveItem.OnOut -= HandleOut;
        ResetButtonVisual();
    }

    private void HandleOut()
    {
        // When the user looks away from the rendering of the scene, hide the radial.
        totalLerpDuration = transitionInDuration;
        countDownToTurnOff = totalLerpDuration;
        lerpA = 1;
        lerpB = 0;
    }

    private void HandleOver()
    {
        totalLerpDuration = transitionOutDuration;
        countDownToTurnOff = totalLerpDuration;
        lerpA = 0;
        lerpB = 1;
        myColor = myImageComponent.color;
    }  

    public void ResetButtonVisual()
    {
        countDownToTurnOff = 0;
        myColor = myImageComponent.color;
        myColor.a = 0;
        myImageComponent.color = myColor;
    }
}
