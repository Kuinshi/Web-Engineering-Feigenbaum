using Fusion;
using UnityEngine;

namespace Networking
{
    public struct TitanNetworkInputData : INetworkInput
    {
        public Vector3 headPosition;
        public Quaternion headRotation;

        public Vector3 leftHandPosition;
        public Quaternion leftHandRotation;
        
        public Vector3 rightHandPosition;
        public Quaternion rightHandRotation;
    }
}
