using UnityEngine;

namespace Addressables
{
    public class UnloadMenuScene : MonoBehaviour
    {
        
        private void Start()
        {
            var loadMenuScene = FindObjectOfType<LoadMenuScene>();
            if(loadMenuScene != null)
                Destroy(loadMenuScene.gameObject);
            Destroy(gameObject);
        }
    
    }
}
