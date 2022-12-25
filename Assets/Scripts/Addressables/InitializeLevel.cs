using System.Collections;
using Fusion;
using Manager;
using Networking;
using UnityEngine;
using UnityEngine.AddressableAssets;
using WebXRAccess;

namespace Addressables
{
    public class InitializeLevel : MonoBehaviour
    {
        [SerializeField] private AssetReference sceneGeometry;

        [SerializeField] private NetworkPrefabRef titanPrefab;
        [SerializeField] private NetworkPrefabRef jaegerPrefab;
        
        
        private void Start()
        {
            StartCoroutine(Load());
        }

        private IEnumerator Load()
        {
            LoadingIndicatorManager.Instance.StartLoading();
            
            var operation = UnityEngine.AddressableAssets.Addressables.InstantiateAsync(sceneGeometry);

            while (!operation.IsDone)
                yield return null;

            if (NetworkRunnerManager.Instance.Runner.IsServer)
                SpawnCharacters();

            var localPLayer = PlayerObject.Local;
            if(localPLayer == null || !localPLayer.IsTitan)
            {
                Debug.Log("Disabling Web VR Rig is local Player is a Jaeger");
                WebVRRig.Instance.gameObject.SetActive(false);
            }   
            
            LoadingIndicatorManager.Instance.StopLoading();

            // ToDo: Mark User as loaded
            // On host, check if everyone is loaded, then start the game with a 3 second Timer via an RPC?
        }

        private void SpawnCharacters()
        {
            foreach (PlayerObject po in PlayerRegistry.Titan)
            {
                NetworkObject playerObject = NetworkRunnerManager.Instance.Runner.Spawn(titanPrefab, Vector3.zero, Quaternion.identity, po.Ref);
                po.playerCharacter = playerObject;
            }

            foreach (PlayerObject po in PlayerRegistry.Jaeger)
            {
                // ToDo: Determine proper Spawn Points for players here.
                NetworkObject playerObject = NetworkRunnerManager.Instance.Runner.Spawn(jaegerPrefab, Vector3.zero, Quaternion.identity, po.Ref);
                po.playerCharacter = playerObject;
            }
        }
    }
    
}
