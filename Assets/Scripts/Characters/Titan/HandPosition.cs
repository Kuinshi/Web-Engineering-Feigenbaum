using UnityEngine;

namespace Characters.Titan
{
    public class HandPosition : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Transform followObject;
        [SerializeField] private Vector3 positionOffset;
        [SerializeField] private Vector3 rotationOffset;
        [SerializeField] private Vector3 localRotationOffset;
        

        private void FixedUpdate()
        {
            Vector3 targetPosition = followObject.position + positionOffset;
            rb.MovePosition(targetPosition);

            Quaternion targetRotation = followObject.rotation * Quaternion.Euler(rotationOffset);
            rb.MoveRotation(targetRotation);
        }
        
    }
}
