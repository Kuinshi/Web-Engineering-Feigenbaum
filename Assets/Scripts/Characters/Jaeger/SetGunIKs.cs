using System.Collections;
using UnityEngine;

namespace Characters.Jaeger
{
    public class SetGunIKs : MonoBehaviour
    {
        [SerializeField] private Transform rightHandFpIk;
        [SerializeField] private Transform leftHandFpIk;

        private Transform rightHandFpTarget;
        private Transform leftHandFpTarget;
        
        private void Start()
        {
            StartCoroutine(SetUp());
        }

        private IEnumerator SetUp()
        {
            JaegerIKTargets ikReferences = transform.root.GetComponentInChildren<JaegerIKTargets>();
            ikReferences.firstPersonRig.enabled = true;
            
            rightHandFpTarget = ikReferences.rightHandFpTarget;
            leftHandFpTarget = ikReferences.leftHandFpTarget;
            
            CopyTransform(rightHandFpIk, rightHandFpTarget);
            CopyTransform(leftHandFpIk, leftHandFpTarget);

            yield return null;
            
            ikReferences.firstPersonRig.enabled = true;
        }
        

        private void CopyTransform(Transform origin, Transform target)
        {
            target.rotation = origin.rotation;
            target.position = origin.position;
        }
    }
}
