using System;
using TMPro;
using UnityEngine;

namespace VRStandardAssets.Utils
{
    // In order to interact with objects in the scene
    // this class casts a ray into the scene and if it finds
    // a VRInteractiveItem it exposes it for other classes to use.
    // This script should be generally be placed on the camera.
    public class VRRaycaster : MonoBehaviour
    {
        public event Action<RaycastHit> OnRaycasthit;                   // This event is called every frame that the user's gaze is over a collider.

        
        [SerializeField] private Transform m_Camera;
        [SerializeField] private LayerMask m_UILayers;                  // Layers to prioritize
        [SerializeField] private LayerMask m_ExclusionLayers;           // Layers to exclude from the raycast.
        [SerializeField] private Reticle m_Reticle;                     // The reticle, if applicable.
        [SerializeField] private VRInput m_VrInput;                     // Used to call input based events on the current VRInteractiveItem.
        [SerializeField] private bool m_ShowDebugRay;                   // Optionally show the debug ray.
        [SerializeField] private float m_DebugRayLength = 5f;           // Debug ray length.
        [SerializeField] private float m_DebugRayDuration = 1f;         // How long the Debug ray will remain visible.
        [SerializeField] private float m_RayLength = 10f;              // How far into the scene the ray is cast.
        [SerializeField] private LineRenderer m_LineRenderer = null; // For supporting Laser Pointer
        public bool ShowLineRenderer = true;                         // Laser pointer visibility
        [SerializeField] private Transform m_TrackingSpace = null;   // Tracking space (for line renderer)


        private VRInteractiveItem m_CurrentInteractible;                //The current interactive item
        private VRInteractiveItem m_LastInteractible;                   //The last interactive item

        RaycastHit[] hits = new RaycastHit[10];

        // Utility for other classes to get the current interactive item
        public VRInteractiveItem CurrentInteractible
        {
            get { return m_CurrentInteractible; }
        }

        private void Awake()
        {
            
        }

        private void OnEnable()
        {
            m_VrInput.OnClick += HandleClick;
            m_VrInput.OnDoubleClick += HandleDoubleClick;
            m_VrInput.OnUp += HandleUp;
            m_VrInput.OnDown += HandleDown;
        }


        private void OnDisable ()
        {
            m_VrInput.OnClick -= HandleClick;
            m_VrInput.OnDoubleClick -= HandleDoubleClick;
            m_VrInput.OnUp -= HandleUp;
            m_VrInput.OnDown -= HandleDown;
        }


        private void Update()
        {
            EyeRaycast();            
        }
        
        public bool ControllerIsConnected
        {
            get
            {
                OVRInput.Controller controller = OVRInput.GetConnectedControllers() & (OVRInput.Controller.LTrackedRemote | OVRInput.Controller.RTrackedRemote);
                return controller == OVRInput.Controller.LTrackedRemote || controller == OVRInput.Controller.RTrackedRemote;
            }
        }
        
        public OVRInput.Controller Controller
        {
            get
            {
                OVRInput.Controller controller = OVRInput.GetConnectedControllers();
                if ((controller & OVRInput.Controller.LTrackedRemote) == OVRInput.Controller.LTrackedRemote)
                {
                    return OVRInput.Controller.LTrackedRemote;
                }
                else if ((controller & OVRInput.Controller.RTrackedRemote) == OVRInput.Controller.RTrackedRemote)
                {
                    return OVRInput.Controller.RTrackedRemote;
                }
                return OVRInput.GetActiveController();
            }
        }
        
        private void EyeRaycast()
        {
            // Show the debug ray if required
            if (m_ShowDebugRay)
            {
                Debug.DrawRay(m_Camera.position, m_Camera.forward * m_DebugRayLength, Color.blue, m_DebugRayDuration);
            }

            // Create a ray that points forwards from the camera.
            Ray ray;            

            Vector3 worldStartPoint;
            Vector3 worldEndPoint;

            switch (PlatformManager.Instance.currentVRPlatform)
            {
                case VRPlataform.PC:
                default:
                    worldStartPoint = m_Camera.position;
                    worldEndPoint = worldStartPoint + (worldStartPoint * m_RayLength);
                    ray = new Ray(m_Camera.position, m_Camera.forward);
                    break;
                case VRPlataform.Oculus:
                    Matrix4x4 localToWorld = m_TrackingSpace.localToWorldMatrix;
                    Quaternion orientation = OVRInput.GetLocalControllerRotation(Controller);

                    Vector3 localStartPoint = OVRInput.GetLocalControllerPosition(Controller);
                    Vector3 localEndPoint = localStartPoint + ((orientation * Vector3.forward) * m_RayLength);

                    worldStartPoint = localToWorld.MultiplyPoint(localStartPoint);
                    worldEndPoint = localToWorld.MultiplyPoint(localEndPoint);

                    // Create new ray
                    ray = new Ray(worldStartPoint, worldEndPoint - worldStartPoint);
                    break;
            }

            //first look for UI elements
            hits = Physics.RaycastAll(ray, m_RayLength, m_UILayers);

            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space))
            {
                foreach (var item in hits)
                {
                    print(item.transform.name +"_" + item.distance);
                }
                print("_______________________");
            }

#endif

            // Do the raycast forwards to see if we hit an interactive item
            if (hits.Length > 0)
            {
                ProcessRaycastHits(hits, worldStartPoint, out worldEndPoint);                

                RenderLine(worldStartPoint, worldEndPoint);

                return;
            }         
            
            //if there is no hit, then look for other colliders
            hits = Physics.RaycastAll(ray, m_RayLength, ~m_ExclusionLayers);

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space))
            {
                foreach (var item in hits)
                {
                    print(item.transform.name);
                }
                print("_______________________");
            }
#endif

            // Do the raycast forwards to see if we hit an interactive item
            if (hits.Length > 0)
            {
                ProcessRaycastHits(hits, worldStartPoint, out worldEndPoint);
            }
            else
            {
                // Nothing was hit, deactive the last interactive item.
                DeactiveLastInteractible();
                m_CurrentInteractible = null;
                // Position the reticle at default distance.
                if (m_Reticle)
                    m_Reticle.SetPosition(ray.origin, ray.direction);
            }

            RenderLine(worldStartPoint, worldEndPoint);

        }

        void ProcessRaycastHits(RaycastHit [] hits, Vector3 worldStartPoint, out Vector3 worldEndPoint)
        {
            VRInteractiveItem interactible = null;
            RaycastHit hit = hits[0];
            float closestInteractive = Mathf.Infinity;

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].distance < closestInteractive)
                {
                    closestInteractive = hits[i].distance;
                    interactible = hits[i].collider.GetComponent<VRInteractiveItem>(); //attempt to get the VRInteractiveItem on the hit object                    
                    hit = hits[i];
                }
            }

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space))
                print(hit.transform.name);
#endif

            m_CurrentInteractible = interactible;
            worldEndPoint = hit.point;

            // If we hit an interactive item and it's not the same as the last interactive item, then call Over
            if (interactible && interactible != m_LastInteractible)
                interactible.Over();

            // Deactive the last interactive item 
            if (interactible != m_LastInteractible)
                DeactiveLastInteractible();

            m_LastInteractible = interactible;

            // Something was hit, set at the hit position.
            if (m_Reticle)
                m_Reticle.SetPosition(hit);

            OnRaycasthit?.Invoke(hit);
        }

        void RenderLine(Vector3 worldStartPoint, Vector3 worldEndPoint)
        {
            if (m_LineRenderer != null && ShowLineRenderer)
            {
                //m_LineRenderer.enabled = ShowLineRenderer;
                m_LineRenderer.SetPosition(0, worldStartPoint);
                m_LineRenderer.SetPosition(1, worldEndPoint);
            }
        }


        private void DeactiveLastInteractible()
        {
            if (m_LastInteractible == null)
                return;

            m_LastInteractible.Out();
            m_LastInteractible = null;
        }


        private void HandleUp()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Up();
        }


        private void HandleDown()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Down();
        }


        private void HandleClick()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Click();
        }


        private void HandleDoubleClick()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.DoubleClick();

        }
    }
}