using Fusion;
using Networking;
using UnityEngine;

namespace Characters.Titan
{
    public class BodyMover : NetworkBehaviour
    {
        [SerializeField] private Transform head;
        [SerializeField] private float maxDistance;
        [SerializeField] private float moveSpeed;

        private Vector3 headPosition;
        private bool shouldMove;

        public override void FixedUpdateNetwork()
        {
            if (!GetInput(out NetworkInputData data))
                return;

            if (shouldMove)
            {
                Vector3 movementVector = headPosition - head.position;
                movementVector.y = 0;
                Vector3 targetPos = movementVector + transform.position;
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveSpeed);
            }

            headPosition = data.vrHeadPosition;
        }
        
        // NOTE: I THINK THIS ONLY WORKS BECAUSE TITAN IS ALWAYS HOST
        private void LateUpdate()
        {
            shouldMove = Vector3.Distance(head.position, headPosition) > maxDistance;
        }
    }
}
