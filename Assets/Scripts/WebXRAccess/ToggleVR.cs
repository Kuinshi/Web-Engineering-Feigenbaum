using UnityEngine;
using WebXR;

namespace WebXRAccess
{
    public class ToggleVR : MonoBehaviour
    {
        public static bool vrOn = false;
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("VR Support? " + WebXRManager.Instance.isSupportedVR);
        }

        public void VR()
        {
            WebXRManager.Instance.ToggleVR();
            vrOn = !vrOn;
        }
    }
}
