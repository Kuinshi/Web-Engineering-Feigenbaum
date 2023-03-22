using System;
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
        private float fuel;
        public float maxFuel = 40;
        
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float speed;
        [SerializeField] private float jumpForce;

        [SerializeField] private Collider groundCheckBox;
        [SerializeField] private LayerMask groundLayer;

        [SerializeField] private Animator animator;
        [SerializeField] private float lerpSpeed;

        [SerializeField] private Transform rotator;
        [SerializeField] private float bodyRotMax;

        [SerializeField] private UpdateWeaponSkin[] weaponSkinScripts;
        [SerializeField] private AudioSource fuelSound;
        

        public event Action<float> OnFuelChanged;

        private bool once;
        private bool lastFrameGrounded;
        private float lastFrameSpeed;
        private static readonly int Forwards = Animator.StringToHash("Forwards");
        private static readonly int Right = Animator.StringToHash("Right");
        private static readonly int FlyingChached = Animator.StringToHash("Flying");
        

        public override void Spawned()
        {
            base.Spawned();
            fuel = maxFuel;
            OnFuelChanged?.Invoke(fuel);

            var ownerId = GetComponent<NetworkObject>().InputAuthority;
            Debug.Log(("Jaeger Prefab Spawned called with ower ID " + ownerId ));
            
            foreach (var po in PlayerRegistry.Jaeger)
            {
                if(po.Ref == ownerId)
                {
                    Debug.Log("Found Matching Player Object");
                    foreach (var weaponSkin in weaponSkinScripts)
                    {
                        weaponSkin.UpdateSkin(po.EquippedSkin);
                    }
                }
            }

            // Set to default layer on clients tht dont have input authority as you want to see the third person view
            if (!HasInputAuthority)
            {
                SetLayerRecursive(gameObject);
            }
        }
        
        private void SetLayerRecursive(GameObject currentGo)
        {
            currentGo.layer = LayerMask.NameToLayer("Default");
            foreach (Transform child in currentGo.transform)
            {
                SetLayerRecursive(child.gameObject);
            }
        }

        private void Update()
        {
            // Collect Input
            if (!HasInputAuthority)
                return;

            NetworkInputBehaviour.Instance.jumpPressed = Input.GetKey(KeyCode.Space);
            if (NetworkInputBehaviour.Instance.jumpPressed)
            {
                fuel -= Time.deltaTime;
                if (fuel <= 0)
                {
                    fuel = 0;
                    NetworkInputBehaviour.Instance.jumpPressed = false;
                }                
                OnFuelChanged?.Invoke(fuel);
            }
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

        private void OnTriggerEnter(Collider other)
        {
            if (!HasInputAuthority)
                return;

            if (!once && other.gameObject.CompareTag("DeathZone"))
            {
                Debug.Log("Collided with Death Zone");
                PlayerObject.Local.PlayerDied();
                GameObject.Find("Death").transform.GetChild(0).gameObject.SetActive(true);
                FindObjectOfType<NetworkRunner>().Despawn(GetComponent<NetworkObject>());
                Destroy(gameObject);
                once = true;
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

            targetVel = rotator.TransformDirection(targetVel);
            Vector3 velocityChange = targetVel - currentVel;
            velocityChange.y = 0;
            rb.AddForce(velocityChange, ForceMode.VelocityChange);

            bool grounded = GroundCheck();

            Flying(data.jumpPressed);
            
            HandleAnimation(data.movementDirection, grounded);

            lastFrameGrounded = grounded;
        }

        private void Flying(bool jumpPressed)
        {
            if (!jumpPressed)
                return;
            
            ApplyFlyForce();
        }

        private void ApplyFlyForce()
        {
            float fallModifier = 1;
            if (rb.velocity.y < -1)
                fallModifier = rb.velocity.y * -1;
            //rb.velocity = velocity;
            rb.AddForce(Vector3.up * jumpForce * fallModifier, ForceMode.Impulse);
        }

        private void HandleAnimation(Vector3 movementDirection, bool grounded)
        {
            float oldForward = animator.GetFloat(Forwards);
            float oldRight = animator.GetFloat(Right);


            animator.SetFloat(Forwards, Mathf.Lerp(oldForward, movementDirection.y, Time.fixedDeltaTime * lerpSpeed));
            animator.SetFloat(Right, Mathf.Lerp(oldRight, movementDirection.x, Time.fixedDeltaTime * lerpSpeed));

            
            if (grounded != lastFrameGrounded)
            {
                animator.SetBool(FlyingChached, !grounded);
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
            return colliders.Length > 2;
            
        }
        

        public void FillFuel()
        {
            fuel = maxFuel;
            Debug.Log("Fuel is now " + fuel);
            fuelSound.Play();
            OnFuelChanged?.Invoke(fuel);
        }

        public void TitanHit(Vector3 hit)
        {
            Rpc_TitanHit(hit);
        }
        
        	
        [Rpc(RpcSources.All, RpcTargets.InputAuthority)]
        void Rpc_TitanHit(Vector3 hit)
        {
            Debug.Log("Executing Titan Hit");
            hit *= PlayerObject.Local.DamagePercent;
            rb.AddForce(hit, ForceMode.Impulse);
        }
    }
}
