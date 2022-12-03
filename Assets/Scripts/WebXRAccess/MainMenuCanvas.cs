using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WebXR;
using Manager;

namespace WebXRAccess
{
    public class MainMenuCanvas : MonoBehaviour
    {
        [SerializeField] private Button vrButton;
        [SerializeField] private CanvasGroup canvasGroup;
        
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
            canvasGroup.alpha = 1;
            
            EventTrigger eventTrigger = vrButton.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry {eventID = EventTriggerType.PointerDown};
            entry.callback.AddListener((evenData) =>
            {
                ActivateVR();
            });
            eventTrigger.triggers.Add(entry);
        }
        
        private void ActivateVR()
        {
            WebXRManager.Instance.ToggleVR();
            LoadScene(true);
        }

        public void LoadScene()
        {
            LoadScene(false);
        }

        private void LoadScene(bool isVR)
        {
            // ToDo: Save the VR Player variable somewhere so it can be asked for when spawning playerprefabs
            SceneManager.Instance.UnloadScene(1);
            SceneManager.Instance.LoadAdditiveScene(2);
        }
    }
}
