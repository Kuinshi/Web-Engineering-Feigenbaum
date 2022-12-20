using UnityEngine;

namespace Characters.Titan
{
    public class BodyMover : MonoBehaviour
    {
        [SerializeField] private Transform headTarget;
        [SerializeField] private Transform head;
        [SerializeField] private float maxDistance;
        [SerializeField] private float moveSpeed;

        private void Update()
        {
            if (Vector3.Distance(head.position, headTarget.position) < maxDistance)
                return;

            Vector3 movementVector = headTarget.position - head.position;
            movementVector.y = 0;
            Vector3 targetPos = movementVector + transform.position;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveSpeed);
        }
    }
}
