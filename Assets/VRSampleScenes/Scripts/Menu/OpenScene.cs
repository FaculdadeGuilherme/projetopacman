using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRStandardAssets.Utils;
using UnityEngine.SceneManagement;


public class OpenScene : ClickSelector
{
    public float lerpTime = 0.5f;
    float countDown;
    public LerpEquationTypes lerp;
    VRCameraFade cameraFade;
    [SerializeField] private string m_SceneToLoad;                      // The name of the scene to load.


    private void Start()
    {
        cameraFade = referenceManager.VRCameraFade;
    }

    //QUANDO O BOTAO É ATIVADO, ESSA FUNCAO EH CHAMADA. AQUI VOCE DEVE PROGRAMAR O SEU CODIGO PARA O SEU BOTAO...
    protected override void ActivateButton()
    {
        StartCoroutine(ChangeSceneWithFadeCamera());
    }
    

    public IEnumerator ChangeSceneWithFadeCamera()//para fazer o fade-in fade-out da camera entre teleporte
    {
        yield return StartCoroutine(cameraFade.BeginFadeOut(lerpTime, false));

        SceneManager.LoadScene(m_SceneToLoad, LoadSceneMode.Single);

    }
}


