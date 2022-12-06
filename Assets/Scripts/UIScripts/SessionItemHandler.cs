using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class SessionItemHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI sessionName;
        [SerializeField] private TextMeshProUGUI sessionCount;
        [SerializeField] private Button joinButton;

        private SessionInfo _sessionInfo;

        public void SetInformation(SessionInfo sessionInfo)
        {
            _sessionInfo = sessionInfo;
            sessionName.text = sessionInfo.Name;
            sessionCount.text = sessionInfo.PlayerCount + "/" + sessionInfo.MaxPlayers;

            joinButton.enabled = sessionInfo.PlayerCount < sessionInfo.MaxPlayers;
        }

        public void JoinClick()
        {
            LobbyBrowser lobbyBrowser = FindObjectOfType<LobbyBrowser>();
            if (lobbyBrowser != null)
            {
                lobbyBrowser.TryJoinSession(_sessionInfo);
            }
            else
            {
                Debug.LogError("Could not find Lobby Browser Script.");
            }
        }
    }
}
