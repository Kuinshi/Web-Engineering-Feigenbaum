using UnityEngine;
using WebXR;

namespace WebXRAccess
{
    public class ToggleVR : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("VR Support? " + WebXRManager.Instance.isSupportedVR);
        }

        public void VR()
        {
            WebXRManager.Instance.ToggleVR();
        }
    }
}
