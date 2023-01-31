using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Util;
using PlayFabScripts;

namespace Manager
{
    public class PlayFabManager : MBSingelton<PlayFabManager>
    {

        private string myId;
        [HideInInspector] public PlayfabWrapper PlayfabWrapper;

        private string lastUsername;
        private string lastPassword;
    
        void Start()
        {
            //titleID von unserem Spiel
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            {
                PlayFabSettings.staticSettings.TitleId = "67BEE";
            }
        }

        #region Login

        //  Username + Passwort wird in PlayerPrefs gespeichert 
        // PlayerId wird in myId gespeichert
        private void OnLoginSuccess(LoginResult result)
        {
        
            PlayerPrefs.SetString("USERNAME", lastUsername);
            PlayerPrefs.SetString("PASSWORD", lastPassword);
            myId = result.PlayFabId;
            PlayerPrefs.SetString("PLAYERID",myId);
            Debug.Log(myId+" wurde eingeloggt");
            Debug.Log("Username = "+ lastUsername);
            
            PlayfabWrapper.OnScucessCallback();
        }

        //to do: zwischen errors differenzieren
        private void OnLoginFailure(PlayFabError error)
        {
            Debug.Log("Login fehlgeschlagen");
            if(PlayfabWrapper != null)
                PlayfabWrapper.ShowPopup(1);
        }
    
    
        //Neues Profil wird angelegt
        private void OnRegisterSuccess(RegisterPlayFabUserResult result)
        {
            PlayerPrefs.SetString("USERNAME", lastUsername);
            PlayerPrefs.SetString("PASSWORD", lastPassword);
            PlayerPrefs.Save();
            myId = result.PlayFabId;
            
            Debug.Log(myId+" wurde registriert");
            Debug.Log("Username = "+ lastUsername);
            
            Login(lastUsername, lastPassword);
        }
    
        private void OnRegisterFailure(PlayFabError error)
        {   
            if (lastPassword.Length < 6)
            {
                if(PlayfabWrapper != null)
                    PlayfabWrapper.ShowPopup(3);
            }
            else
            {
                if(PlayfabWrapper != null)
                    PlayfabWrapper.ShowPopup(2);
            }
            Debug.Log("Fehler beim registrieren");
            Debug.LogError(error.GenerateErrorReport());
        
        }
    
        //Button Funktionen zum einloggen, ausloggen, registrieren
    
        public void Login(string username, string userPassword)
        {
            lastUsername = username;
            lastPassword = userPassword;
            
            LoginWithPlayFabRequest request = new LoginWithPlayFabRequest { Username = username, Password =  userPassword};
            PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginFailure);
        }

        public void Logout()
        {
            PlayFabClientAPI.ForgetAllCredentials();
        }
    
        public void Register(string username, string userPassword)
        {
            lastUsername = username;
            lastPassword = userPassword;
            
            if (CheckInputsValid(username, userPassword))
            {
                var registerRequest = new RegisterPlayFabUserRequest { Username = username, Password = userPassword, RequireBothUsernameAndEmail = false};
                PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);
            }
            else
            {
            
                Debug.Log("Fehler");
                Debug.Log("Username: "+username);
                Debug.Log("Passwort: "+userPassword);
            }
        }

        private Boolean CheckInputsValid(string username, string userPassword)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userPassword))
            {
                return false;
            }

            return true;
        }
        #endregion Login

   
    }
    
}