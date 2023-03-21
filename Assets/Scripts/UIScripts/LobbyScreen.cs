using System.Collections.Generic;
using System.Linq;
using Fusion;
using Manager;
using Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebXR;

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
        [HideInInspector] public bool oldReadyState;
        
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
                readyButton.gameObject.SetActive(false);

            }
            else
            {
                readyButton.gameObject.SetActive(true);
                startGameButton.gameObject.SetActive(false);

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
#if !UNITY_EDITOR
            Debug.Log("Trying to enter VR");
            if(PlayerObject.Local != null && PlayerObject.Local.IsTitan)
                WebXRManager.Instance.ToggleVR();
#endif
            // Change Titan HP
            foreach (var po in PlayerRegistry.Titan)
            {
                po.Health = PlayerRegistry.Jaeger.Count() * 150;
                Debug.Log("Titan health should now be " + po.Health);
            }
            
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
               {
                   po.Rpc_SetReadyState(isReady);
                   po.ChangeSkin(ShopManager.EQUIP);
               }            }
        }

        public void SetReadyViaShop(bool targetState)
        {
            oldReadyState = isReady;
            isReady = targetState;
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
