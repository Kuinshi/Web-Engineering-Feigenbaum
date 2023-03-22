using System;
using System.Linq;
using Fusion;
using UnityEngine;

namespace Characters.Titan
{
    public class FistController : NetworkBehaviour
    {

        [SerializeField] private HandStructure leftHand;
        [SerializeField] private HandStructure rightHand;
        [SerializeField] private float throwForceMultiplier;
        

        private Rigidbody leftGrab;
        private Vector3 leftLastPos;
        private Vector3 leftNewPos;
        
        private Rigidbody rightGrab;
        private Vector3 rightLastPos;
        private Vector3 rightNewPos;

        private void Start()
        {
            if (!HasInputAuthority)
                return;
            
            if (FistPressedEvents.Instance != null)
            {
                FistPressedEvents.Instance.OnLeftFistClosed += OnLeftHandClosed;
                FistPressedEvents.Instance.OnLeftHandOpened += OnLeftHandOpened;

                
                FistPressedEvents.Instance.OnRightFistClosed += OnRightHandClosed;
                FistPressedEvents.Instance.OnRightHandOpened += OnRightHandOpened;
            }
        }

        private void OnLeftHandClosed()
        {
            leftHand.handAnimator.SetBool("Fist", true);
            leftHand.openHandCollider.SetActive(false);
            leftHand.fistCollider.SetActive(true);
            leftHand.openHandTrigger.SetActive(false);
            leftHand.fistTrigger.SetActive(true);
            Grab(leftHand, true);
            Debug.Log("Should be closing left Fist!");
        }

        private void OnRightHandClosed()
        {
            rightHand.handAnimator.SetBool("LOL", true);
            rightHand.openHandCollider.SetActive(false);
            rightHand.fistCollider.SetActive(true);
            rightHand.openHandTrigger.SetActive(false);
            rightHand.fistTrigger.SetActive(true);
            Grab(rightHand, false);
            Debug.Log("Should be closing right Fist!");
        }

        private void OnLeftHandOpened()
        {
            leftHand.handAnimator.SetBool("Fist", false);
            leftHand.openHandCollider.SetActive(true);
            leftHand.fistCollider.SetActive(false);
            leftHand.openHandTrigger.SetActive(true);
            leftHand.fistTrigger.SetActive(false);
            Throw(true);
            Debug.Log("Should be opening left Hand!");
        }

        private void OnRightHandOpened()
        {
            rightHand.handAnimator.SetBool("LOL", false);
            rightHand.openHandCollider.SetActive(true);
            rightHand.fistCollider.SetActive(false);
            rightHand.openHandTrigger.SetActive(true);
            rightHand.fistTrigger.SetActive(false);
            Throw(false);
            Debug.Log("Should be opening right Hand!");
        }

        private void Grab(HandStructure hand, bool isLeft)
        {
            GrabTrigger grabTrigger = isLeft ? leftHand.grabTrigger : rightHand.grabTrigger;
            Transform grabCenter = isLeft ? leftHand.grabCenter : rightHand.grabCenter;

            if (grabTrigger.grabbableObjects.Count <= 0)
                return;

            float baseDist = Vector3.Distance(grabCenter.position ,grabTrigger.grabbableObjects[0].position);
            var rb = grabTrigger.grabbableObjects[0];

            for (int i = 1; i < grabTrigger.grabbableObjects.Count; i++)
            {
                float newDist = Vector3.Distance(grabCenter.position ,grabTrigger.grabbableObjects[i].position);
                if (newDist < baseDist)
                {
                    baseDist = newDist;
                    rb = grabTrigger.grabbableObjects[i];
                }
            }
            
            Debug.Log("Grabbed: " + rb.name);
            rb.isKinematic = true;
            rb.useGravity = false;
            
            if (isLeft)
            {
                leftGrab = rb;
                leftLastPos = rb.position;
                leftNewPos = hand.grabCenter.position;
            }                
            else
            {
                rightGrab = rb;
                rightLastPos = rb.position;
                rightNewPos = hand.grabCenter.position;
            }              
            
            rb.position = hand.grabCenter.position;
        }

        private void Throw(bool left)
        {
            Debug.Log("Trying to throw!");
            Rigidbody throwable = left ? leftGrab : rightGrab;
            Vector3 oldPos = left ? leftLastPos : rightLastPos;
            Vector3 newPos = left ? leftNewPos : rightNewPos;

            if (throwable == null)
            {
                Debug.Log("Nothing to throw");
                return;
            }

            throwable.isKinematic = false;
            throwable.useGravity = true;
            Vector3 force = (newPos - oldPos) * throwForceMultiplier;
            throwable.AddForce(force, ForceMode.Impulse);
            Debug.Log("Throwing with Force of: " + force);

            if (left)
                leftGrab = null;
            else
                rightGrab = null;
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasInputAuthority)
                return;

            if (leftGrab != null)
            {
                leftLastPos = leftNewPos;
                leftNewPos = leftHand.grabCenter.position;
                leftGrab.position = leftNewPos;

            }            
            
            if (rightGrab != null)
            {
                rightLastPos = rightNewPos;
                rightNewPos = rightHand.grabCenter.position;
                rightGrab.position = rightNewPos;
            }        
        }

        private void OnDestroy()
        {
            if (!HasInputAuthority)
                return;
            
            if (FistPressedEvents.Instance != null)
            {
                FistPressedEvents.Instance.OnLeftFistClosed -= OnLeftHandClosed;
                FistPressedEvents.Instance.OnRightFistClosed -= OnRightHandClosed;
                FistPressedEvents.Instance.OnLeftHandOpened -= OnLeftHandOpened;
                FistPressedEvents.Instance.OnRightHandOpened -= OnRightHandOpened;
            }
        }
    }

    [Serializable]
    public class HandStructure
    {
        public Animator handAnimator;
        public GameObject openHandCollider;
        public GameObject openHandTrigger;
        public GameObject fistCollider;
        public GameObject fistTrigger;
        public GrabTrigger grabTrigger;
        public Transform grabCenter;
    }
    
}
