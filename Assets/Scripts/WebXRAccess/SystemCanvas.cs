using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WebXR;

namespace WebXRAccess
{
    public class SystemCanvas : MonoBehaviour
    {
        [SerializeField] private Button vrButton;

        private bool _isFullscreen;

        private void Start()
        {
#if UNITY_EDITOR
            // WebXR Stuff only works in Browser :c
            return;
#endif

            Debug.Log("Checking VR Support!");
            if (!WebXRManager.Instance.isSupportedVR)
                return;
        
            vrButton.enabled = true;
            EventTrigger eventTrigger = vrButton.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry {eventID = EventTriggerType.PointerDown};
            entry.callback.AddListener((evenData) =>
            {
                ActivateVR();
            });
            eventTrigger.triggers.Add(entry);
        }

        public void Fullscreen()
        {
            _isFullscreen = !_isFullscreen;
            Screen.fullScreen = _isFullscreen;
        }

        private void ActivateVR()
        {
            WebXRManager.Instance.ToggleVR();
        }
    
    }
}
