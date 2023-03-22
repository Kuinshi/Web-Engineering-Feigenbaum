using System;
using UnityEngine;
using WebXR;
using WebXRAccess;

namespace Characters.Titan
{
    public class FistPressedEvents : MonoBehaviour
    {
        public static FistPressedEvents Instance;
        
        [SerializeField] private WebXRController leftController;
        [SerializeField] private WebXRController rightController;

        public event Action OnRightFistClosed;
        public event Action OnLeftFistClosed;
        public event Action OnRightHandOpened;
        public event Action OnLeftHandOpened;

        private void Awake()
        {

            if(Instance != null)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        private void Update()
        {

            if (!ToggleVR.vrOn)
                return;

            if (leftController.GetButtonDown(WebXRController.ButtonTypes.Trigger)
                || leftController.GetButtonDown(WebXRController.ButtonTypes.Grip)
                || leftController.GetButtonDown(WebXRController.ButtonTypes.ButtonA))
            {

                Debug.Log("CLosing Left Fist");
                OnLeftFistClosed?.Invoke();
            }
            
            if (leftController.GetButtonUp(WebXRController.ButtonTypes.Trigger)
                || leftController.GetButtonUp(WebXRController.ButtonTypes.Grip)
                || leftController.GetButtonUp(WebXRController.ButtonTypes.ButtonA))
            {
                Debug.Log("Opening Left Hand");
                OnLeftHandOpened?.Invoke();
            }
            
            if (rightController.GetButtonDown(WebXRController.ButtonTypes.Trigger)
                || rightController.GetButtonDown(WebXRController.ButtonTypes.Grip)
                || rightController.GetButtonDown(WebXRController.ButtonTypes.ButtonA))
            {
                Debug.Log("Closing Right Fist");
                OnRightFistClosed?.Invoke();
            }
            
            if (rightController.GetButtonUp(WebXRController.ButtonTypes.Trigger)
                || rightController.GetButtonUp(WebXRController.ButtonTypes.Grip)
                || rightController.GetButtonUp(WebXRController.ButtonTypes.ButtonA))
            {
                Debug.Log("Opening Right Hand");
                OnRightHandOpened?.Invoke();
            }
        }
#endif
        
#if UNITY_EDITOR
        // Debug Function to debug VR Fists opening/Closing
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Debug.Log("CLosing Left Fist");
                OnLeftFistClosed?.Invoke();
            }
            
            if (Input.GetKeyUp(KeyCode.K))
            {
                Debug.Log("Opening Left Hand");
                OnLeftHandOpened?.Invoke();
            }
            
            if (Input.GetKeyDown(KeyCode.L))
            {
                Debug.Log("Closing Right Fist");
                OnRightFistClosed?.Invoke();
            }
            
            if (Input.GetKeyDown(KeyCode.L))
            {
                Debug.Log("Opening Right Hand");
                OnRightHandOpened?.Invoke();
            }
        }
#endif
    }
}
