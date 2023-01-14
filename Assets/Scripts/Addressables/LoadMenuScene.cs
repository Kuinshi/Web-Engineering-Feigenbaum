using Manager;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Addressables  
{
    public class LoadMenuScene : MonoBehaviour
    {
        [SerializeField] private AssetReference scene;

        
        private void Start()
        {
            LoadingIndicatorManager.Instance.StartLoading();
            scene.LoadSceneAsync();
        }

        private void Update()
        {
            if (scene.IsDone)
            {
                LoadingIndicatorManager.Instance.StopLoading();
                Destroy(gameObject);
            }
        }
    }
}
