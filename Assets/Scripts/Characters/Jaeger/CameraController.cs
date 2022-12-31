using System;
using Fusion;
using UnityEngine;

namespace Characters.Jaeger
{
    public class CameraController : NetworkBehaviour
    {
        [SerializeField] private Transform bodyRoot;
        [SerializeField] private Transform cameraHead;
        [SerializeField] private float mouseSpeed;
        [SerializeField] private float xRotLimit;
        [SerializeField] private bool debugMode;

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
            
            var mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            
            bodyRoot.Rotate(Vector3.up, mouseDelta.x * mouseSpeed);

            currentXrot += (-mouseDelta.y * mouseSpeed);
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
