using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRStandardAssets.Utils;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Script usado para trocar de cena a partir de um botao
//
//
public class ChangeScene : MonoBehaviour
{
    public float lerpTime = 0.5f;
    float countDown;
    public LerpEquationTypes lerp;
    VRCameraFade cameraFade;
    [SerializeField] private string m_SceneToLoad;                      // The name of the scene to load.
    [SerializeField] string buttonToHold; //o nome do botao para apertar, configurado no Input da Unity
    [SerializeField] float timeToHold = 2; //o tempo necessario para segurar o botao para concluir a ação (0 seria instantaneo)
    float count;
    private Image m_Selection;


    private void Start()
    {
        cameraFade = ReferenceManager.Instance.VRCameraFade;
        m_Selection = ReferenceManager.Instance.SelectionRadialSlider;
    }

    private void Update()
    {
        if (Input.GetButton(buttonToHold))
            count += Time.deltaTime;
        else
        {
            count = 0;
            m_Selection.fillAmount = 0;
        }
        // Setup the radial to have no fill at the start and hide if necessary.
        if (count > 0)
        {
            m_Selection.gameObject.SetActive(true);
            m_Selection.fillAmount = count / timeToHold;            
        }
        if (count >= timeToHold)
        {
            count = 0;
            StartCoroutine(ChangeSceneWithFadeCamera());
        }
    }    

    public IEnumerator ChangeSceneWithFadeCamera()//para fazer o fade-in fade-out da camera entre teleporte
    {
        yield return StartCoroutine(cameraFade.BeginFadeOut(lerpTime, false));

        SceneManager.LoadScene(m_SceneToLoad, LoadSceneMode.Single);

    }
}


