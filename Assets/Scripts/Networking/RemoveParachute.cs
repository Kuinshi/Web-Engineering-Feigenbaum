using System.Collections;
using Fusion;
using Manager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Networking
{
    public class RemoveParachute : NetworkBehaviour
    {

        [SerializeField] private float tweenTime = 0.5f;
        [SerializeField] private GameObject parachute;
        [SerializeField] private Rigidbody rb;
        

        private bool parachuteDeactived;
        private Vector3 baseScale;

        public override void Spawned()
        {
            rb.drag = Random.Range(2.0f, 5.0f);
            baseScale = transform.localScale;
            transform.localScale = Vector3.zero;
            StartCoroutine(ScaleUp());
        }

        private IEnumerator ScaleUp()
        {
            float timeGoneBy = 0;

            while (timeGoneBy <= tweenTime)
            {
                yield return null;
                timeGoneBy += Time.deltaTime;

                float percent = timeGoneBy / tweenTime;
                transform.localScale = Vector3.Lerp(Vector3.zero, baseScale, percent);
            }
        }

        public void Use()
        {
            Rpc_Despawn();
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        void Rpc_Despawn()
        {
            if (NetworkRunnerManager.Instance.Runner.IsServer)
            {
                var runner = FindObjectOfType<NetworkRunner>();
                runner.Despawn(GetComponent<NetworkObject>());
            }

            Destroy(gameObject);
        }
    
        private void OnCollisionEnter(Collision other)
        {
            if(!parachuteDeactived)
                Rpc_RemoveParachute();
        }


        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        void Rpc_RemoveParachute()
        {
            parachute.SetActive(false);
            parachuteDeactived = true;
        }
    }
}
