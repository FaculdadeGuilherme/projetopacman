using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

//Usada para tocar um som a partir do clique em um objeto
public class SoundClickSelector : ClickSelector
{
    private AudioSource audioSource;

    [SerializeField] SoundSelector soundSelector;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    protected override void ActivateButton()
    {
        soundSelector.PlaySound(audioSource);
    }
}
