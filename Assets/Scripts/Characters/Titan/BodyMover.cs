using Fusion;
using Networking;
using UnityEngine;

namespace Characters.Titan
{
    [DefaultExecutionOrder(-1000)]
    public class BodyMover : NetworkBehaviour
    {
        [SerializeField] private Transform head;
        [SerializeField] private float maxDistance;
        [SerializeField] private float moveSpeed;

        
        public override void FixedUpdateNetwork()
        {
            if (!GetInput(out NetworkInputData data))
                return;

            if (Vector3.Distance(head.position, data.vrHeadPosition) < maxDistance)
                return;

            Vector3 movementVector = data.vrHeadPosition - head.position;
            movementVector.y = 0;
            Vector3 targetPos = movementVector + transform.position;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveSpeed);
        }
    }
}
