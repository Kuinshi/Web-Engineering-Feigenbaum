using Fusion;
using Networking;
using UnityEngine;

namespace Characters.Titan
{
    public class HandPosition : NetworkBehaviour
    {
        [SerializeField] private bool isLeftHand;
        
        [SerializeField] private Rigidbody rb;
        
        [SerializeField] private Vector3 positionOffset;
        [SerializeField] private float handMovespeed = 777;
        
        [SerializeField] private Vector3 rotationOffset;
        
        

        public override void FixedUpdateNetwork()
        {
            if (!GetInput(out NetworkInputData data))
                return;
            
            Vector3 targetPosition = isLeftHand ? data.leftHandPosition  : data.rightHandPosition;
            targetPosition += positionOffset;
            float distance = Vector3.Distance(targetPosition, transform.position);
            Vector3 direction = (targetPosition - transform.position).normalized;
            rb.velocity = direction * distance * handMovespeed * Time.fixedDeltaTime;

            Quaternion targetRotation = isLeftHand ? data.leftHandRotation : data.rightHandRotation;
            targetRotation *= Quaternion.Euler(rotationOffset);
            rb.MoveRotation(targetRotation);
        }
        
    }
}
