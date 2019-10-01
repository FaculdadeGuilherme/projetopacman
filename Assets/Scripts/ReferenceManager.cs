using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using VRStandardAssets.Utils;

public class ReferenceManager : SingletonGameObject<ReferenceManager>
{
    Image selectionRadialSlider;    
    VRCameraFade vrCameraFade;
    Transform player;
    PlatformManager platformManager;

    public Image SelectionRadialSlider { get => selectionRadialSlider;}    
    public VRCameraFade VRCameraFade { get => vrCameraFade;}
    public Transform Player { get => player; }
    public PlatformManager PlatformManager { get => platformManager; }

    [SerializeField] GameObject pcCam, oculusCam;

    [SerializeField] AudioSource audioSource;
    // Start is called before the first frame update
    void Awake()
    {
        base.Awake();
        FindAndSetPlatform();

        FindAssets();        
        
        StartCoroutine(WaitBeforeFaidOut());

    }

    void FindAssets()
    {
        selectionRadialSlider = GameObject.Find("UISelectionBar")?.GetComponent<Image>();
        if (!SelectionRadialSlider)
            Debug.LogError("UISelectionBar not found");
        SelectionRadialSlider.gameObject.SetActive(false);

        vrCameraFade = (VRCameraFade)FindObjectOfType(typeof(VRCameraFade));
        if (!VRCameraFade)
            Debug.LogError("VRCameraFade not found");

        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (!Player)
            Debug.LogError("Player not found");
        
        if(!audioSource)
            Debug.LogError("Audio source not found");
    }
    
    private IEnumerator WaitBeforeFaidOut()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(vrCameraFade.BeginFadeIn(3, false));
    }

    private void FindAndSetPlatform()
    {
        platformManager = PlatformManager.Instance;
        if (!PlatformManager)
            Debug.LogError("Platform Manager not found");

        bool isPc = false, isOculus = false;
        switch (PlatformManager.currentVRPlatform)
        {
            case VRPlataform.PC:
            default:
                isPc = true;
                break;
            case VRPlataform.Oculus:
                isOculus = true;
                break;
        }
        pcCam?.SetActive(isPc);
        oculusCam?.SetActive(isOculus);
    }
    
    public void PlayAudioClip(AudioClip audioClip, float volume)
    {
        audioSource.volume = volume;
        audioSource.PlayOneShot(audioClip);
    }

    public void PlayAudioClip(AudioClip audioClip)
    {
        audioSource.volume = 0.1f;
        audioSource.PlayOneShot(audioClip);
    }
}
