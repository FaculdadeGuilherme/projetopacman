using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

//Usada para tocar um som apos passar o mouse em um objeto
public class SoundHoverSelector : HoverSelector
{
    private AudioSource audioSource;

    [SerializeField] SoundSelector soundSelector;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected override void HandleOver()
    {
        soundSelector.PlaySound(audioSource);
    }
}
