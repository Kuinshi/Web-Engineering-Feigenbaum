using UnityEngine;

namespace Characters.Titan
{
    public class HeadPosition : MonoBehaviour
    {
        [SerializeField] private Transform followObject;
        [SerializeField] private Vector3 positionOffset;
        [SerializeField] private Vector3 rotationOffset;

        private void Update()
        {
            transform.position = followObject.position + positionOffset;
            transform.rotation = Quaternion.Euler(followObject.rotation.eulerAngles + rotationOffset);
        }
    }
}
