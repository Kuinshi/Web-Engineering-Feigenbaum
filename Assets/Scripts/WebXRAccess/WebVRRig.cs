using Networking;
using UnityEngine;
using Util;

namespace WebXRAccess
{
    public class WebVRRig : MBSingelton<WebVRRig>
    {
        public Transform head;
        public Transform leftHand;
        public Transform rightHand;

        public NetworkInputData GetTitanInput()
        {
            NetworkInputData networkInputData = new NetworkInputData();
            
            networkInputData.headRotation = head.rotation;
            networkInputData.vrHeadPosition = head.position;

            networkInputData.leftHandPosition = leftHand.position;
            networkInputData.leftHandRotation = leftHand.rotation;

            networkInputData.rightHandPosition = rightHand.position;
            networkInputData.rightHandRotation = rightHand.rotation;

            return networkInputData;
        }
    }
}
