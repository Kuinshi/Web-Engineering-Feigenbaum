using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using Manager;
using Networking;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Util;

namespace UIScripts
{
    public class LobbyBrowser : MonoBehaviour, INetworkRunnerCallbacks
    {
        [Header("Join Lobby")]
        [SerializeField] private UnityEvent onLobbyConnected;
        [SerializeField] private DisconnectUI disconnectUi;

        [Header("Lobby List")] 
        [SerializeField] private SessionItemHandler sessionItemPrefab;
        [SerializeField] private Transform sessionItemParent;
        [SerializeField] private TextMeshProUGUI statusText;

        [SerializeField] private GameObject NetworkManager;
            
        [SerializeField] private UnityEvent onRoomJoined;

        
        
        #region Display Lobby Browser

        public void TryJoinLobbyBrowser()
        {
            StartCoroutine(JoinLobbyBrowser());
        }

        private IEnumerator JoinLobbyBrowser()
        {
            LoadingIndicatorManager.Instance.StartLoading();

            statusText.text = "Looking for active lobbies...";
            NetworkRunnerManager.Instance.Runner.AddCallbacks(this);
            Task<StartGameResult> task =  NetworkRunnerManager.Instance.Runner.JoinSessionLobby(SessionLobby.ClientServer);
            while (!task.IsCompleted)
            {
                yield return null;
            }
            StartGameResult result = task.Result;
		
            if (result.Ok)
            {
                Debug.Log("Connected to lobby.");
                onLobbyConnected?.Invoke();
            }
            else
            {
                disconnectUi.CallDisconnect(result.ShutdownReason.ToString());
            }
            
            LoadingIndicatorManager.Instance.StopLoading();
        }

        #endregion

        #region Lobby Browser Functions
        
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            Debug.Log("Lobby Browser active");
            
            ClearList();

            Debug.Log("Session list count is : " + sessionList.Count);
            if (sessionList.Count == 0)
            {
                statusText.text = "No active lobbies found!";
                return;
            }

            foreach (var session in sessionList)
            {
                AddToList(session);
            }
            
            statusText.gameObject.SetActive(false);
        }
        
        private void ClearList()
        {
            // ToDo: Memory wise it would be better to pool this, but I am too lazy right now
            foreach (Transform child in sessionItemParent)
            {
                Destroy(child.gameObject);
            }
        }

        private void AddToList(SessionInfo sessionInfo)
        {
            SessionItemHandler sessionItem = Instantiate(sessionItemPrefab, sessionItemParent);
            sessionItem.SetInformation(sessionInfo);
        }
        #endregion
        
        public void TryJoinSession(SessionInfo sessionInfo)
        {
            StartCoroutine(JoinSessionRoutine(sessionInfo));
        }

        IEnumerator JoinSessionRoutine(SessionInfo sessionInfo)
        {
            LoadingIndicatorManager.Instance.StartLoading();

            LocalPlayerData.isVrPlayer = false;

            Task<StartGameResult> task = NetworkRunnerManager.Instance.Runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Client,
                SessionName = sessionInfo.Name,
                SceneManager = NetworkRunnerManager.Instance.Runner.GetComponent<INetworkSceneManager>(),
                DisableClientSessionCreation = true
            });
            while (!task.IsCompleted)
            {
                yield return null;
            }
            StartGameResult result = task.Result;

            if (result.Ok)
            {
                onRoomJoined?.Invoke();
            }
            else
            {
                disconnectUi.CallDisconnect(result.ShutdownReason.ToString());
            }
            
            LoadingIndicatorManager.Instance.StopLoading();
        }
        
        #region HostSession

        public void TryHostSession()
        {
            StartCoroutine(HostSessionRoutine());
        }

        private IEnumerator HostSessionRoutine()
        {
            LoadingIndicatorManager.Instance.StartLoading();
            
            NetworkRunnerManager.Instance.Runner.GetComponent<NetworkEvents>().PlayerJoined.AddListener((runner, player) =>
            {
                if (runner.IsServer && runner.LocalPlayer == player)
                {
                    runner.Spawn(NetworkManager);
                }
            });
            NetworkRunnerManager.Instance.Runner.AddCallbacks(this);


            string code = RandomStringGenerator.GetRandomString(6);
            LocalPlayerData.isVrPlayer = true;

            Task<StartGameResult> task = NetworkRunnerManager.Instance.Runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Host,
                SessionName = code,
                PlayerCount = 5,
                SceneManager = NetworkRunnerManager.Instance.Runner.GetComponent<INetworkSceneManager>()
            });
            while (!task.IsCompleted)
            {
                yield return null;
            }
            
            StartGameResult result = task.Result;
            if (result.Ok)
            {
                onRoomJoined?.Invoke();
            }
            else
            {
                disconnectUi.CallDisconnect(result.ToString());
            }
            
            LoadingIndicatorManager.Instance.StopLoading();
        }
        
        #endregion
        

        
        #region NetworkRunnerCallbacks
            public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
            {
            }

            public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
            {
            }

            public void OnInput(NetworkRunner runner, NetworkInput input)
            {
            }

            public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
            {
            }

            public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
            {
            }

            public void OnConnectedToServer(NetworkRunner runner)
            {
            }

            public void OnDisconnectedFromServer(NetworkRunner runner)
            {
            }

            public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
            {
            }

            public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
            {
            }

            public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
            {
            }

            public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
            {
            }

            public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
            {
            }

            public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
            {
            }

            public void OnSceneLoadDone(NetworkRunner runner)
            {
            }

            public void OnSceneLoadStart(NetworkRunner runner)
            {
            }
        #endregion
    }
}
