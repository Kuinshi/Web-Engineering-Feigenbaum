using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Util;

namespace Manager
{
    public class SceneManager : MBSingelton<SceneManager>
    {

        private List<int> _activeAdditiveScenes = new List<int>();
        
        private void Start()
        {
            // Automatically load Main Menu 
            LoadAdditiveScene(1);
        }

        // ToDo: Implement other functions to laod/unload scenes, list of active scenes etc.

        public void LoadAdditiveScene(int index)
        {
            if (_activeAdditiveScenes.Contains(index))
                return;
            
            UnityEngine.SceneManagement.SceneManager.LoadScene(index, LoadSceneMode.Additive);
            _activeAdditiveScenes.Add(index);
        }
        
        public void LoadAdditiveScene(string sceneName)
        {
            int index = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName).buildIndex;
            LoadAdditiveScene(index);
        }

        public void UnloadScene(int index)
        {
            if (!_activeAdditiveScenes.Contains(index))
                return;
            
            _activeAdditiveScenes.Remove(index);
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(index);
        }
        
        public void UnloadScene(string sceneName)
        {
            int index = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName).buildIndex;
            UnloadScene(index);
        }
    }
}
