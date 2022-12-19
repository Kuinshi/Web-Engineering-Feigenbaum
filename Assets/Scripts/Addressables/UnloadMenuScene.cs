using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Addressables
{
    public class UnloadMenuScene : MonoBehaviour
    {
        [SerializeField] private AssetReference scene;

    
        private void Start()
        {
            scene.UnLoadScene();
        }
    
    }
}
