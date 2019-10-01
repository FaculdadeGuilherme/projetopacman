using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

//
//Usada para ativar e desativar uma animacao a partir do clique em um objeto
[RequireComponent(typeof(Animation))]
public class AnimationSelector : ClickSelector
{
    private Animation anim;
    string animationName;
    bool isOn = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        animationName = anim.clip.name;
        foreach (AnimationState state in anim)
        {
            animationName = state.name;
        }
    }

    protected override void ActivateButton()
    {
        if(isOn)
        {
            isOn = !isOn;
            anim[animationName].speed = -1;
            anim[animationName].time = anim.isPlaying ? anim[animationName].time : anim[animationName].length;
            anim.Play(animationName);
        }
        else
        {
            isOn = !isOn;
            anim[animationName].speed = 1;
            anim.Play();
            print(anim.isPlaying);
        }
    }
}
