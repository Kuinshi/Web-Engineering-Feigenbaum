using UnityEngine;

namespace Characters.Jaeger
{
    public class SetGunIKs : MonoBehaviour
    {
        [SerializeField] private Transform rightHandFpIk;
        [SerializeField] private Transform leftHandFpIk;

        private Transform rightHandFpTarget;
        private Transform leftHandFpTarget;
        
        private bool isSetUp;

        private void Start()
        {
            JaegerIKTargets ikReferences = GetComponentInParent<JaegerIKTargets>();
            
            rightHandFpTarget = ikReferences.rightHandFpTarget;
            leftHandFpTarget = ikReferences.leftHandFpTarget;
            
            isSetUp = true;
        }

        private void Update()
        {
            if (!isSetUp)
                return;
            
            CopyTransform(rightHandFpIk, rightHandFpTarget);
            CopyTransform(leftHandFpIk, leftHandFpTarget);
        }

        private void CopyTransform(Transform origin, Transform target)
        {
            target.rotation = origin.rotation;
            target.position = origin.position;
        }
    }
}
