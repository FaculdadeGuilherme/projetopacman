using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

//Usada para tocar um audio
[System.Serializable]
public class SoundSelector 
{

    [SerializeField] AudioClip audioClip;
    [Tooltip("Play just once or until deactivation")]
    public bool playOnce = true;
    bool isOn;

    [SerializeField] float volume = 0.5f;
        
    public void PlaySound(AudioSource audioSource)
    {
        if (playOnce)
        {
            ReferenceManager.Instance.PlayAudioClip(audioClip, volume);
        }
        else
        {
            if (isOn)
            {
                audioSource.Stop();
                isOn = !isOn;
            }
            else
            {
                audioSource.Play();
                isOn = !isOn;
            }
        }

    }
}
