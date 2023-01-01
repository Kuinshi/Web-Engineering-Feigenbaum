using System.Numerics;
using Fusion;
using Networking;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Characters.Jaeger
{
    public class CharacterMover : NetworkBehaviour
    {
        [SerializeField] private bool debugMode;

        [SerializeField] private Rigidbody rb;
        [SerializeField] private float speed;
        [SerializeField] private float jumpForce;
        
        [SerializeField] private Collider groundCheckBox;
        [SerializeField] private LayerMask groundLayer;

        [SerializeField] private Animator animator;
        [SerializeField] private float lerpSpeed;

        [SerializeField] private Transform rotator;
        [SerializeField] private float bodyRotMax;
        
        

        private bool lastFrameGrounded;
        private float lastFrameSpeed;

        private void Start()
        {
            if(debugMode)
                Spawned();
        }

        private void Update()
        {
            // Collect Input
            if (!HasInputAuthority && !debugMode)
                return;

            NetworkInputBehaviour.Instance.jumpPressed = Input.GetKey(KeyCode.Space);

            Vector2 debugMovement = Vector2.zero;
            if (Input.GetKey(KeyCode.W))
                debugMovement.y += 1;
            if (Input.GetKey(KeyCode.S))
                debugMovement.y -= 1;
            if (Input.GetKey(KeyCode.D))
                debugMovement.x += 1;
            if (Input.GetKey(KeyCode.A))
                debugMovement.x -= 1;

            NetworkInputBehaviour.Instance.movementDirection = debugMovement;
        }

        private void FixedUpdate()
        {
            if (debugMode)
            {
                NetworkInputData data = NetworkInputBehaviour.Instance.GetJaegerInput();
                Move(data);
            }
        }

        public override void FixedUpdateNetwork()
        {

            if (GetInput(out NetworkInputData data))
            {
                Move(data);
            }
        }

        private void Move(NetworkInputData data)
        {
            Vector3 currentVel = rb.velocity;
            Vector3 targetVel = new Vector3(data.movementDirection.x, 0, data.movementDirection.y);
            targetVel *= speed;

            targetVel = transform.TransformDirection(targetVel);
            Vector3 velocityChange = targetVel - currentVel;
            velocityChange.y = 0;
            rb.AddForce(velocityChange, ForceMode.VelocityChange);

            bool grounded = GroundCheck();


            if (data.jumpPressed) // ToDo: Add Fuel Resource and check!
            {
                rb.AddForce(Vector3.up*jumpForce, ForceMode.VelocityChange);
            }
            
            HandleAnimation(data.movementDirection, grounded);
        
            lastFrameGrounded = grounded;
        }

        private void HandleAnimation(Vector3 movementDirection, bool grounded)
        {
            float targetSpeed = movementDirection.magnitude;
            if (targetSpeed > 1)
                targetSpeed = 1;

            float oldSpeed = animator.GetFloat("Blend");

            animator.SetFloat("Blend", Mathf.Lerp(oldSpeed, targetSpeed, Time.fixedDeltaTime * lerpSpeed));
            
            if (grounded != lastFrameGrounded)
            {
                if (grounded)
                {
                    animator.SetTrigger("Land");
                }
                else
                {
                    animator.SetTrigger("Jump");
                }
            }

            Vector3 targetRot = Vector3.zero;

            if (!grounded)
            {
                if (movementDirection.y >= 1)
                    targetRot.x = bodyRotMax;
                
                if(movementDirection.y <= -1)
                    targetRot.x = -bodyRotMax;

                if (movementDirection.x >= 1)
                    targetRot.z = -bodyRotMax;
                
                if(movementDirection.x <= -1)
                    targetRot.z = bodyRotMax;
            }

            rotator.localRotation = Quaternion.Slerp(rotator.localRotation, Quaternion.Euler(targetRot),
                Time.fixedDeltaTime * lerpSpeed);
        }
        
        
        private bool GroundCheck()
        {
            var colliders = Physics.OverlapBox(groundCheckBox.bounds.center, groundCheckBox.transform.localScale, Quaternion.identity, groundLayer);
            return colliders.Length > 1;
            
        }
    }
}
