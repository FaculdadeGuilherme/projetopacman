using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum VRPlataform
{
    PC, 
    Oculus
}

public class PlatformManager : SingletonGameObject<PlatformManager>
{
    public VRPlataform currentVRPlatform = VRPlataform.PC;
    string loadedDeviceName; 

    // Start is called before the first frame update
    public void Awake()
    {
        base.Awake();
        if (XRDevice.isPresent)
        {         
            OnDeviceLoadAction(XRDevice.model);
        }
        else
        {
            XRDevice.deviceLoaded += OnDeviceLoadAction;
        }
        //if (currentVRPlatform == VRPlataform.PC)
        //    Cursor.visible = false;
    }

    void OnDeviceLoadAction(string newLoadedDeviceName)
    {
        loadedDeviceName = newLoadedDeviceName;        
        if (loadedDeviceName.ToLower().Contains("oculus"))
        {
            currentVRPlatform = VRPlataform.Oculus;
        }
    }

}
