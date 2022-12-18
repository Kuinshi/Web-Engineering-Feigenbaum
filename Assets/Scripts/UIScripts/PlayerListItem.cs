using System.Linq;
using Fusion;
using Networking;
using TMPro;
using UnityEngine;

namespace UIScripts
{
    public class PlayerListItem : NetworkBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameField;
        [SerializeField] private TextMeshProUGUI readyField;
        public bool isJaeger;

        private PlayerObject _player;

        public PlayerObject Player
        {
            get
            {
                if (_player == null)
                    _player = PlayerRegistry.GetPlayer(Object.InputAuthority);
                return _player;
            }
        }

        public override void Spawned()
        {
            isJaeger = !Player.IsTitan;
            Transform parent = isJaeger ? FindObjectOfType<LobbyScreen>().jaegerList : FindObjectOfType<LobbyScreen>().titanList;
            transform.SetParent(parent);
            transform.localScale = Vector3.one;
            Player.OnStatChanged += UpdateStats;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            Player.OnStatChanged -= UpdateStats;
        }

        private void UpdateStats()
        {
            SetName(Player.Nickname);
            SetReadyField(Player.IsReady);
            UpdateStartGameButton();
        }


        private void SetName(string username)
        {
            nameField.text = username;
        }

        private void SetReadyField(bool isReady)
        {
            if (!isJaeger)
            {
                readyField.text = "Host";
            }
            else
            {
                readyField.text = isReady ? "YES" : "NO";
            }
        }

        private void UpdateStartGameButton()
        {
            var lobbyScreen = FindObjectOfType<LobbyScreen>();
            if (lobbyScreen == null)
                return;

            bool buttonClickable = false;

            if (PlayerRegistry.Jaeger.Any())
            {
                buttonClickable = true;
                foreach (var jaeger in PlayerRegistry.Jaeger)
                {
                    buttonClickable = buttonClickable && jaeger.IsReady;
                }
            }
            
            lobbyScreen.SetStartVisibility(buttonClickable);
        }
    }
}
