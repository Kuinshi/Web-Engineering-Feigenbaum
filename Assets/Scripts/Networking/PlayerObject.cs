using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Networking
{
	/// <summary>
	/// Class copied from Photon Fusion's Minigolf Example and adjusted to our needs
	/// </summary>
	public class PlayerObject : NetworkBehaviour
	{
		public static PlayerObject Local { get; private set; }

		private static bool once;
		
		// Metadata
		[Networked] public PlayerRef Ref { get; set; }
		[Networked] public byte Index { get; set; }
		

		
		// User Settings
		[Networked(OnChanged = nameof(StatChanged))] public string Nickname { get; set; }
		[Networked(OnChanged = nameof(StatChanged))] public bool IsReady { get; set; }
		[Networked(OnChanged = nameof(StatChanged))] public string EquippedSkin { get; set; }

		[Networked] public bool IsTitan { get; private set; }
		

		
		// State & Gameplay Info
		[Networked] public bool IsLoaded { get; set; }
		[Networked(OnChanged = nameof(GameOverCheck))] public bool IsDead { get; set; }

		[Networked] public int Health { get; set; } = 1000;


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

		public void ChangeSkin(string skinId)
		{
			Rpc_SetEquippedSkin(skinId);
		}
		
		[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
		void Rpc_SetNickname(string nick)
		{
			Nickname = nick;
		}
		
		[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
		void Rpc_SetEquippedSkin(string skinId)
		{
			EquippedSkin = skinId;
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
		
		[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
		void Rpc_Died()
		{
			IsDead = true;
		}

		public void TakeDamage(int damageValue)
		{
			Rpc_Damage(damageValue);
		}
		
		[Rpc(RpcSources.All, RpcTargets.InputAuthority)]
		void Rpc_Damage(int damageValue)
		{
			Health -= damageValue;

			if (Health <= 0)
			{
				PlayerDied();
			}
		}

		public void PlayerDied()
		{
			Rpc_Died();
		}

		static void StatChanged(Changed<PlayerObject> p)
		{
			p.Behaviour.OnStatChanged?.Invoke();
		}

		static void GameOverCheck(Changed<PlayerObject> p)
		{
			if (once)
			{
				return;
			}
			
			
			
			// Check Titan Dead
			bool titanDead = true;
			foreach (var titan in PlayerRegistry.Titan)
			{
				if (!titan.IsDead)
				{
					titanDead = false;
				}			
			}

			if (titanDead)
			{
				Debug.Log("GAME OVER - TITAN DIED - JAEGER WON");
				once = true;
				SceneManager.LoadScene("03.2_GameEnd_JaegerWon");
				FindObjectOfType<NetworkRunner>().Shutdown();
				return;
			}
			
			
			
			// Check Jaeger Dead
			bool jaegerDead = true;
			foreach (var jaeger in PlayerRegistry.Jaeger)
			{
				if (!jaeger.IsDead)
					jaegerDead = false;
			}

			if (jaegerDead)
			{
				Debug.Log("GAME OVER - ALL JAEGERS DEAD - TITAN WON");
				once = true;
				SceneManager.LoadScene("03.1_GameEnd_TitanWon");
				FindObjectOfType<NetworkRunner>().Shutdown();
			}
		}
	}
}
