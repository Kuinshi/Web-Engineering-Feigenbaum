using System;
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
        private static readonly int Fist = Animator.StringToHash("Fist");

        private void Start()
        {
            if (!HasInputAuthority)
                return;
            
            if (FistPressedEvents.Instance != null)
            {
                FistPressedEvents.Instance.OnLeftFistClosed += OnLeftHandClosed;
                FistPressedEvents.Instance.OnRightFistClosed += OnRightHandClosed;
                FistPressedEvents.Instance.OnLeftHandOpened += OnLeftHandOpened;
                FistPressedEvents.Instance.OnRightHandOpened += OnRightHandOpened;
            }
        }

        private void OnLeftHandClosed()
        {
            leftHand.handAnimator.SetBool(Fist, true);
            leftHand.openHandCollider.SetActive(false);
            leftHand.fistCollider.SetActive(true);
            Grab(leftHand, true);
            Debug.Log("Should be closing left Fist!");
        }

        private void OnRightHandClosed()
        {
            rightHand.handAnimator.SetBool(Fist, true);
            rightHand.openHandCollider.SetActive(false);
            rightHand.fistCollider.SetActive(true);
            Grab(rightHand, false);
            Debug.Log("Should be closing right Fist!");
        }

        private void OnLeftHandOpened()
        {
            leftHand.handAnimator.SetBool(Fist, false);
            leftHand.openHandCollider.SetActive(true);
            leftHand.fistCollider.SetActive(false);
            Throw(true);
            Debug.Log("Should be opening left Hand!");
        }

        private void OnRightHandOpened()
        {
            rightHand.handAnimator.SetBool(Fist, false);
            rightHand.openHandCollider.SetActive(true);
            rightHand.fistCollider.SetActive(false);
            Throw(false);
            Debug.Log("Should be opening right Hand!");
        }

        private void Grab(HandStructure hand, bool isLeft)
        {
            Debug.Log("Trying to Grab");
            Collider[] hitColliders = Physics.OverlapSphere(hand.grabTrigger.transform.position, hand.grabTrigger.transform.localScale.x*0.5f);
            foreach (var collider in hitColliders)
            {
                var grabbable = collider.GetComponent<Grabbable>();
                if (grabbable == null)
                    continue;

                var rb = grabbable.GetComponent<Rigidbody>();
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

                return;
            }
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
            }

            throwable.isKinematic = false;
            throwable.useGravity = true;
            Vector3 force = (newPos - oldPos) * throwForceMultiplier;
            throwable.AddForce(force, ForceMode.Impulse);
            Debug.Log("Throwing with Force of: " + force);
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
        public GameObject fistCollider;
        public GameObject grabTrigger;
        public Transform grabCenter;
    }
    
}
