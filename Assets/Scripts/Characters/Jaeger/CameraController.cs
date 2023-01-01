using Fusion;
using Networking;
using UnityEngine;

namespace Characters.Jaeger
{
    public class CameraController : NetworkBehaviour
    {
        [SerializeField] private bool debugMode;

        [SerializeField] private Transform rotator;
        [SerializeField] private Transform cameraRotator;
        
        [SerializeField] private Transform cameraHead;
        
        [SerializeField] private float mouseSpeed;
        [SerializeField] private float xRotLimit;

        private float currentXrot;

        private void Start()
        {
            if(debugMode)
                Spawned();
        }

        public override void Spawned()
        {
            base.Spawned();
            Debug.Log("Setting camera to: " + (HasInputAuthority || debugMode));
            cameraHead.gameObject.SetActive(HasInputAuthority || debugMode);

            if (HasInputAuthority || debugMode)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        private void Update()
        {
            if (!HasInputAuthority && !debugMode)
                return;
            
            RotateCamera(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
        }

        private void FixedUpdate()
        {
            if (debugMode)
            {
                NetworkInputData data = NetworkInputBehaviour.Instance.GetJaegerInput();
                RotateBody(data);
            }
        }

        public override void FixedUpdateNetwork()
        {

            if (GetInput(out NetworkInputData data))
            {
                RotateBody(data);
            }
            
        }

        private void RotateCamera(Vector2 mouseDelta)
        {
            cameraRotator.Rotate(Vector3.up, mouseDelta.x * mouseSpeed);
            
            currentXrot += (-mouseDelta.y * mouseSpeed);
            currentXrot = Mathf.Clamp(currentXrot, -xRotLimit, xRotLimit);
            cameraHead.localRotation = Quaternion.Euler(new Vector3(currentXrot, 0, 0));
            NetworkInputBehaviour.Instance.yBodyRotation = cameraRotator.localRotation.eulerAngles.y;
        }

        private void RotateBody(NetworkInputData data)
        {
            Vector3 oldLocRot = rotator.localRotation.eulerAngles;
            oldLocRot.y = data.yBodyRotation;
            rotator.localRotation = Quaternion.Euler(oldLocRot);
        }

        private void OnDestroy()
        {
            if (HasInputAuthority || debugMode)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
