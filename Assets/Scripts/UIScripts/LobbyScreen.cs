using System.Collections.Generic;
using Fusion;
using Manager;
using Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class LobbyScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI roomname;

        [SerializeField] private PlayerListItem playerListItem;
        
        public Transform jaegerList;
        public Transform titanList;

        [SerializeField] private Button startGameButton;
        [SerializeField] private CanvasGroup startGameCanvasGroup;
        
        [SerializeField] private Button readyButton;

        readonly Dictionary<PlayerRef, PlayerListItem> playerItems = new Dictionary<PlayerRef, PlayerListItem>();
        private bool isReady;

        
        public void AddSubscriptions()
        {
            PlayerRegistry.OnPlayerJoined += PlayerJoined;
            PlayerRegistry.OnPlayerLeft += PlayerLeft;
        }
        
        private void OnEnable()
        {
            PlayerRegistry.OnPlayerJoined -= PlayerJoined;
            PlayerRegistry.OnPlayerLeft -= PlayerLeft;
            PlayerRegistry.OnPlayerJoined += PlayerJoined;
            PlayerRegistry.OnPlayerLeft += PlayerLeft;

            roomname.text = "Roome Name: " + NetworkRunnerManager.Instance.Runner.SessionInfo.Name;

            if (NetworkRunnerManager.Instance.Runner.IsServer)
            {
                startGameButton.gameObject.SetActive(true);
            }
            else
            {
                readyButton.gameObject.SetActive(true);
            }
        }

        private void OnDisable()
        {
            PlayerRegistry.OnPlayerJoined -= PlayerJoined;
            PlayerRegistry.OnPlayerLeft -= PlayerLeft;
            
            playerItems.Clear();

            foreach (Transform child in titanList)
            {
                Destroy(child.gameObject);
            }

            foreach (Transform child in jaegerList)
            {
                Destroy(child.gameObject);
            }
        }

        private void PlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (playerItems.TryGetValue(player, out PlayerListItem item))
            {
                if (item)
                {
                    Debug.Log($"Removing Player List Item for {player}");
                    NetworkRunnerManager.Instance.Runner.Despawn(item.Object);
                }
                else
                {
                    Debug.Log($"{nameof(PlayerListItem)} for {player} was null.");
                }
                
                playerItems.Remove(player);
            }
            else
            {
                Debug.LogWarning($"{player} not found");
            }
        }

        public void PlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log("Lobby Screen received Player Joined event for " + player);
            if (playerItems.ContainsKey(player))
            {
                Debug.Log("Player already listed!");
            }
            else
            {
                PlayerListItem item = NetworkRunnerManager.Instance.Runner.Spawn(
                    playerListItem, inputAuthority: player);
                
                playerItems.Add(player, item);
            }
        }

        public void StartGame()
        {
            NetworkRunnerManager.Instance.Runner.SetActiveScene("02_Game");
            
            var playerListItems = FindObjectsOfType<PlayerListItem>();
            foreach (var listItem in playerListItems)
            {
                NetworkRunnerManager.Instance.Runner.Despawn(listItem.Object);
            }
        }

        public void ReadyClick()
        {
            isReady = !isReady;
            readyButton.GetComponentInChildren<TextMeshProUGUI>().text = isReady ? "Not ready" : "Ready";

            foreach (var po in PlayerRegistry.Everyone)
            {
               if(po.Ref == NetworkRunnerManager.Instance.Runner.LocalPlayer)
                   po.Rpc_SetReadyState(isReady);
            }
        }

        public void SetStartVisibility(bool visible)
        {
            startGameButton.interactable = visible;
            startGameCanvasGroup.alpha = visible ? 1 : 0.5f;
        }
    }
}
