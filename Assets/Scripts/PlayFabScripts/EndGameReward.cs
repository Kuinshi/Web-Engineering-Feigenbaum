using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Titan;
using Manager;
using Networking;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using WebXR;
using WebXRAccess;

namespace PlayFabScripts
{
    public class EndGameReward : MonoBehaviour
    {
        private int coins = 0;
        private bool gotData;
        
        // Start is called before the first frame update
        private IEnumerator Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
#if UNITY_WEBGL && !UNITY_EDITOR
            if (FistPressedEvents.VrOn)
            {
                Debug.Log("Trying to turn off VR");
                WebXRManager.Instance.ToggleVR();
            }
#endif

            GetOldCoins();

            while (!gotData)
            {
                yield return null;
            }
            
            SetCoins(coins + 10);
        }

        private void GetOldCoins()
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest() {
                PlayFabId = PlayFabManager.PlayerId,
                Keys = null
            }, result => {
                Debug.Log("Got user data:");
                if (result.Data.ContainsKey("coins"))
                {
                    coins = Int32.Parse(result.Data["coins"].Value);
                    GotData();
                }

            }, (error) => {
                Debug.Log("Got error retrieving user data:");
                Debug.Log(error.GenerateErrorReport());
            });
        }

        private void GotData()
        {
            gotData = true;
        }

        private void SetCoins(int newCoins)
        {
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
            {
                Data = new Dictionary<string, string>()
                {
                    {"coins",newCoins.ToString()}
                }
            }, SetDataSuccess,SetDataFailure);
        }
        
        void SetDataSuccess(UpdateUserDataResult result)
        {
            Debug.Log(result.DataVersion);
        }
        void SetDataFailure(PlayFabError error)
        {
            Debug.Log(error.GenerateErrorReport());
        }
    }
}
