using System;
using Fusion;
using Networking;
using UnityEngine;

namespace Characters.Jaeger
{
    public class CameraController : NetworkBehaviour
    {
        [SerializeField] private bool debugMode;

        [SerializeField] private Transform bodyRoot;
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
            
            NetworkInputBehaviour.Instance.mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            
            // Save Input
        }

        private void FixedUpdate()
        {
            if (debugMode)
            {
                NetworkInputData data = NetworkInputBehaviour.Instance.GetJaegerInput();
                RotateCamera(data);
            }
        }

        public override void FixedUpdateNetwork()
        {

            if (GetInput(out NetworkInputData data))
            {
                RotateCamera(data);
            }
            
        }

        private void RotateCamera(NetworkInputData data)
        {
            bodyRoot.Rotate(Vector3.up, data.mouseDelta.x * mouseSpeed);

            currentXrot += (-data.mouseDelta.y * mouseSpeed);
            currentXrot = Mathf.Clamp(currentXrot, -xRotLimit, xRotLimit);
            cameraHead.localRotation = Quaternion.Euler(new Vector3(currentXrot, 0, 0));
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
