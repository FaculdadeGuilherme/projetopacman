using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;

/// <summary>Usada para mexer dinamicamente em propriedades de um objeto da GUI,
/// como posição, rotação e escala, alem tambem da cor do objeto .</summary>
public class LerpUIElement : MonoBehaviour {

    [Header("Lerp General configuration")]
    public LerpEquationTypes lerp;
    public float transitionInDuration = 0.5f, transitionOutDuration = 0.5f;
    
    float lerpFactor, lerpA, lerpB;
    float totalLerpDuration;
    protected float countDownToTurnOff;

    [Header("Lerp Transform configuration")]
    public bool lerpTransform = true;
    public Vector3 desiredLocalPosition;
    public Vector3 desiredLocalRotation;
    public Vector3 desiredLocalScale;

    [Header("Lerp Color configuration")]
    public bool lerpColor = true;
    public Color desiredColor = Color.black;

    Vector3 initialLocalPosition, initialLocalRotation, initialLocalScale; 
    Color initialColor;

    Vector3 lerpPosA, lerpPosB, lerpEulerA, lerpEulerB, lerpScaleA, lerpScaleB;
    Color lerpColorA, lerpColorB;

    [Header("VRInteractiveItem reference")]
    [SerializeField] protected VRInteractiveItem m_InteractiveItem;       // The interactive item for where the user should click to load the level.
    Image myImage;
    // Use this for initialization
    void Awake () {
        if(m_InteractiveItem == null)
           m_InteractiveItem = GetComponent<VRInteractiveItem>();

        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localEulerAngles;
        initialLocalScale = transform.localScale;
        myImage = GetComponent<Image>();
        if(myImage)
            initialColor =myImage.color;
    }

    void Update()
    {
        countDownToTurnOff -= Time.deltaTime;
        if (countDownToTurnOff > 0)
        {
            lerpFactor = countDownToTurnOff / totalLerpDuration;

            if (lerpTransform)
            {
                if (desiredLocalPosition != Vector3.zero)
                    transform.localPosition = lerp.Lerp(lerpPosA, lerpPosB, 1 - lerpFactor);

                if (desiredLocalRotation != Vector3.zero)
                    transform.localEulerAngles = lerp.Lerp(lerpEulerA, lerpEulerB, 1 - lerpFactor);

                if (desiredLocalScale != Vector3.zero)
                    transform.localScale = lerp.Lerp(lerpScaleA, lerpScaleB, 1 - lerpFactor);
            }

            if (lerpColor && desiredColor != Color.black && myImage)
                myImage.color = lerp.Lerp(lerpColorA, lerpColorB, 1 - lerpFactor);
        }
    }


    private void OnEnable()
    {
        m_InteractiveItem.OnOver += HandleOver;
        m_InteractiveItem.OnOut += HandleOut;
        Reset();
    }


    private void OnDisable()
    {
        m_InteractiveItem.OnOver -= HandleOver;
        m_InteractiveItem.OnOut -= HandleOut;
        Reset();
    }

    private void HandleOut()
    {
        // When the user looks away from the rendering of the scene, hide the radial.
        totalLerpDuration = transitionInDuration;
        countDownToTurnOff = totalLerpDuration;        

        lerpPosA = desiredLocalPosition;
        lerpPosB = initialLocalPosition;
        lerpEulerA = desiredLocalRotation;
        lerpEulerB = initialLocalRotation;
        lerpScaleA = desiredLocalScale;
        lerpScaleB = initialLocalScale;
        lerpColorA = desiredColor;
        lerpColorB = initialColor;
    }

    private void HandleOver()
    {
        totalLerpDuration = transitionOutDuration;
        countDownToTurnOff = totalLerpDuration;

        lerpPosA = initialLocalPosition;
        lerpPosB = desiredLocalPosition;
        lerpEulerA = initialLocalRotation;
        lerpEulerB = desiredLocalRotation;
        lerpScaleA = initialLocalScale;
        lerpScaleB = desiredLocalScale;
        lerpColorA = initialColor;
        lerpColorB = desiredColor;
    }

    public void Reset()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            return;
#endif     
        countDownToTurnOff = 0;
        transform.localPosition = initialLocalPosition;
        transform.localEulerAngles = initialLocalRotation;
        transform.localScale = initialLocalScale;
    }
}
