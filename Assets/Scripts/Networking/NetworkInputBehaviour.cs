using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.Serialization;
using WebXRAccess;
using Behaviour = Fusion.Behaviour;

namespace Networking
{
    public class NetworkInputBehaviour : Behaviour, INetworkRunnerCallbacks
    {
        public static NetworkInputBehaviour Instance;
        
        public Vector2 mouseDelta;
        public Vector2 movementDirection;
        public bool jumpPressed;


        private void Awake()
        {
            if(Instance != null)
                Destroy(this);

            Instance = this;
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            if (PlayerObject.Local == null)
                return;

            if (PlayerObject.Local.IsTitan)
            {
                if (WebVRRig.Instance == null)
                    return;

                NetworkInputData networkInputData = WebVRRig.Instance.GetTitanInput();
                input.Set(networkInputData);
            }
            else
            {
                input.Set(GetJaegerInput());
            }
        }

        public NetworkInputData GetJaegerInput()
        {
            NetworkInputData networkInputData = new NetworkInputData();

            networkInputData.mouseDelta = mouseDelta;
            networkInputData.movementDirection = movementDirection;
            networkInputData.jumpPressed = jumpPressed;
            
            return networkInputData;
        }
    
        
    


        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
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

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
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
    }
}
