using Fusion;
using UnityEngine;

namespace Characters.Titan
{
    public class HeadPosition : NetworkBehaviour
    {
        [SerializeField] private Transform followObject;
        [SerializeField] private Vector3 positionOffset;
        [SerializeField] private Vector3 rotationOffset;

        public override void FixedUpdateNetwork()
        {
            if (followObject == null)
                return;
            Debug.Log("Fixed Update Network is run!");

            

            
            transform.position = followObject.position + positionOffset;
            transform.rotation = Quaternion.Euler(followObject.rotation.eulerAngles + rotationOffset);
        }
    }
}
