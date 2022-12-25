using Fusion;
using UnityEngine;

namespace Characters.Titan
{
    public class HandPosition : NetworkBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Transform followObject;
        
        [SerializeField] private Vector3 positionOffset;
        [SerializeField] private float handMovespeed = 777;
        
        [SerializeField] private Vector3 rotationOffset;
        
        

        public override void FixedUpdateNetwork()
        {
            if (followObject == null)
                return;
            Debug.Log("Fixed Update Network is run!");

            
            
            
            
            Vector3 targetPosition = followObject.position + positionOffset;
            float distance = Vector3.Distance(targetPosition, transform.position);
            Vector3 direction = (targetPosition - transform.position).normalized;
            rb.velocity = direction * distance * handMovespeed * Time.fixedDeltaTime;

            Quaternion targetRotation = followObject.rotation * Quaternion.Euler(rotationOffset);
            rb.MoveRotation(targetRotation);
        }
        
    }
}
