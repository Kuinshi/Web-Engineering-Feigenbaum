using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Addressables
{
    public class LoadLevel : MonoBehaviour
    {

        [SerializeField] private AssetReference sceneGeometry;
        
        private void Start()
        {
            Debug.Log("Streaming Asset Path:");
            Debug.Log(Application.streamingAssetsPath);
            StartCoroutine(Load());
        }

        private IEnumerator Load()
        {
            var operation = UnityEngine.AddressableAssets.Addressables.InstantiateAsync(sceneGeometry);

            while (!operation.IsDone)
                yield return null;

        }
    }
}
