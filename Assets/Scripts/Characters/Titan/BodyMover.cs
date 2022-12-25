using Fusion;
using UnityEngine;

namespace Characters.Titan
{
    public class BodyMover : NetworkBehaviour
    {
        public Transform headTarget;
        [SerializeField] private Transform head;
        [SerializeField] private float maxDistance;
        [SerializeField] private float moveSpeed;

        public override void FixedUpdateNetwork()
        {
            if (headTarget == null)
                return;
            Debug.Log("Fixed Update Network is run!");

            
            
            if (Vector3.Distance(head.position, headTarget.position) < maxDistance)
                return;

            Vector3 movementVector = headTarget.position - head.position;
            movementVector.y = 0;
            Vector3 targetPos = movementVector + transform.position;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveSpeed);
        }
    }
}
