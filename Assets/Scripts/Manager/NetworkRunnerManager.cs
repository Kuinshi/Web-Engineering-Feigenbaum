using Fusion;
using UnityEngine;
using Util;

namespace Manager
{
    public class NetworkRunnerManager : MBSingelton<NetworkRunnerManager>
    {
        [SerializeField] private NetworkRunner runnerPrefab;

        public NetworkRunner Runner { get; private set; }

        protected override void Init()
        {
            StartRunner();
        }

        public void StartRunner()
        {
            Runner = Instantiate(runnerPrefab, transform);
            Debug.Log("Runner instantiated");
        }

        public void StopRunner()
        {
            Destroy(Runner);
            Runner = null;
        }

    }
}
