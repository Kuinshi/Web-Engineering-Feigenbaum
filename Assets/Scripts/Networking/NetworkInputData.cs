using Fusion;
using UnityEngine;

namespace Networking
{
    public struct NetworkInputData : INetworkInput
    {
        #region TitanPlayer

        public Vector3 vrHeadPosition;
        public Quaternion headRotation;

        public Vector3 leftHandPosition;
        public Quaternion leftHandRotation;
        
        public Vector3 rightHandPosition;
        public Quaternion rightHandRotation;

        #endregion

    }
}
