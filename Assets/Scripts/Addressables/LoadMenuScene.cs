using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Addressables  
{
    public class LoadMenuScene : MonoBehaviour
    {
        [SerializeField] private AssetReference scene;

        
        private void Start()
        {
            scene.LoadSceneAsync();
            Destroy(gameObject);
        }
    }
}
