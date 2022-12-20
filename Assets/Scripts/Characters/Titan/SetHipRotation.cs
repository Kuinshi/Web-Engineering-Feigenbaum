using UnityEngine;

namespace Characters.Titan
{
    public class SetHipRotation : MonoBehaviour
    {
        [SerializeField] private Transform hipTarget;
        [SerializeField] private Transform headTarget;
        [SerializeField] private float rotationSpeed;
        

        private void LateUpdate()
        {
            Vector3 direction = headTarget.position - hipTarget.position;
            Quaternion lookDirection = Quaternion.LookRotation(direction, Vector3.up);
            lookDirection = Quaternion.Euler(new Vector3(0, lookDirection.eulerAngles.y, 0));
            hipTarget.rotation = Quaternion.Slerp(hipTarget.rotation, lookDirection, Time.deltaTime * rotationSpeed);
        }
    }
}
