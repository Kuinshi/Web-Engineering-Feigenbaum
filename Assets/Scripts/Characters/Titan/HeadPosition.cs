using Fusion;
using Networking;
using UnityEngine;

namespace Characters.Titan
{
    public class HeadPosition : NetworkBehaviour
    {
        [SerializeField] private Vector3 positionOffset;
        [SerializeField] private Vector3 rotationOffset;

        public override void FixedUpdateNetwork()
        {
            if (!GetInput(out NetworkInputData data))
                return;

            transform.position = data.vrHeadPosition + positionOffset;
            transform.rotation = Quaternion.Euler(data.headRotation.eulerAngles + rotationOffset);
        }
    }
}
