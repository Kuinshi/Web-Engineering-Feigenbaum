using UnityEngine;

namespace Characters.Titan
{
    public class HandPosition : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Transform followObject;
        
        [SerializeField] private Vector3 positionOffset;
        [SerializeField] private float handMovespeed = 777;
        
        [SerializeField] private Vector3 rotationOffset;
        
        

        private void FixedUpdate()
        {
            
            /*
            Vector3 targetPosition = followObject.position + positionOffset;
            rb.MovePosition(targetPosition);
            */
                        
            Vector3 targetPosition = followObject.position + positionOffset;
            float distance = Vector3.Distance(targetPosition, transform.position);
            Vector3 direction = (targetPosition - transform.position).normalized;
            rb.velocity = direction * distance * handMovespeed * Time.fixedDeltaTime;
            
            
            
            Quaternion targetRotation = followObject.rotation * Quaternion.Euler(rotationOffset);
            rb.MoveRotation(targetRotation);
            
            /*
            Quaternion targetRotation = followObject.rotation * Quaternion.Euler(rotationOffset);
            Quaternion rotValue = targetRotation * Quaternion.Inverse(rb.rotation);
            rotValue.ToAngleAxis(out float angle, out Vector3 axis);
            rb.angularVelocity = axis * (angle * Mathf.Deg2Rad * handRotspeed * Time.fixedDeltaTime);
            */
            
        }
        
    }
}
