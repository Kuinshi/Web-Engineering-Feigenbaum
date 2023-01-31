using UIScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadShopScene : MonoBehaviour
{
    [SerializeField] private string shopSceneName;

    public void Load()
    {
        SceneManager.LoadScene(shopSceneName, LoadSceneMode.Additive);
    }
    
    public void Unload()
    {
        SceneManager.UnloadSceneAsync(shopSceneName);
        var lobbyScreen = FindObjectOfType<LobbyScreen>();
        if(lobbyScreen != null)
            lobbyScreen.SetReadyViaShop(lobbyScreen.oldReadyState);
    }
}