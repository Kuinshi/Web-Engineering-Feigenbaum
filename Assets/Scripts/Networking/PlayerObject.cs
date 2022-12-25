using Fusion;
using UnityEngine;

namespace Networking
{
	/// <summary>
	/// Class copied from Photon Fusion's Minigolf Example and adjusted to our needs
	/// </summary>
	public class PlayerObject : NetworkBehaviour
	{
		public static PlayerObject Local { get; private set; }
	
		
		// Metadata
		[Networked] public PlayerRef Ref { get; set; }
		[Networked] public byte Index { get; set; }
		

		
		// User Settings
		[Networked(OnChanged = nameof(StatChanged))] public string Nickname { get; set; }
		[Networked(OnChanged = nameof(StatChanged))] public bool IsReady { get; set; }
		[Networked] public bool IsTitan { get; private set; }
		

		
		// State & Gameplay Info
		[Networked] public bool IsLoaded { get; set; }
		public NetworkObject playerCharacter;

		
		// Events
		public event System.Action OnStatChanged;

		public void Server_Init(PlayerRef pRef, byte index)
		{
			Debug.Assert(Runner.IsServer);

			Ref = pRef;
			Index = index;
		}

		public override void Spawned()
		{
			if (Object.HasStateAuthority)
			{
				PlayerRegistry.Server_Add(Runner, Object.InputAuthority, this);
			}
			
			if (Object.HasInputAuthority)
			{
				Local = this;
				Rpc_SetNickname(LocalPlayerData.userName);
				Rpc_SetPlayerMode(LocalPlayerData.isVrPlayer);
			}

			Debug.Log("Player Object Spawned for Player " + Object.InputAuthority);
			PlayerRegistry.PlayerJoined(Object.InputAuthority);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if (Local == this) Local = null;

			if (!runner.IsShutdown)
			{
				Debug.Log($"Player Object {Ref.PlayerId} desapwned!");
			}

		}
		
		[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
		void Rpc_SetNickname(string nick)
		{
			Nickname = nick;
		}
		
		[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
		public void Rpc_SetReadyState(bool readyState)
		{
			IsReady = readyState;
			Debug.Log("ReadyState updated to " + readyState);
		}
		
		[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
		void Rpc_SetPlayerMode(bool isTitan)
		{
			IsTitan = isTitan;
		}

		static void StatChanged(Changed<PlayerObject> p)
		{
			p.Behaviour.OnStatChanged?.Invoke();
		}
		
	}
}
