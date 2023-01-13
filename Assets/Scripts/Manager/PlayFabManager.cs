using System;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager PFM;

    private string username;
    private string userPassword;
    private string myId;

    public GameObject loginPanel;

    public GameObject popupPanel;

    public TextMeshProUGUI popupText;
    
    // Start is called before the first frame update
    void Start()
    {
        //titleID von unserem Spiel
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "67BEE";
        }
        hidePopup();
    }

    #region Login

    //  Username + Passwort wird in PlayerPrefs gespeichert 
    // PlayerId wird in myId gespeichert
    private void OnLoginSuccess(LoginResult result)
    {
        
        PlayerPrefs.SetString("USERNAME",username);
        PlayerPrefs.SetString("PASSWORD",userPassword);

        myId = result.PlayFabId;
        
        Debug.Log(myId+" wurde eingeloggt");
        Debug.Log("Username = "+username);
    }

    //to do: Pop up mit Error; Anbieten zu registrieren
    private void OnLoginFailure(PlayFabError error)
    {
        Debug.Log("Login fehlgeschlagen");
        showPopup("Login");
    }
    
    
    //Neues Profil wird angelegt
    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        PlayerPrefs.SetString("USERNAME",username);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        myId = result.PlayFabId;
        
        Debug.Log(myId+" wurde registriert");
        Debug.Log("Username = "+username);

    }

    //maybe auch ein Popup
    private void OnRegisterFailure(PlayFabError error)
    {
        showPopup("Register");
        Debug.Log("Fehler beim registrieren");
        Debug.LogError(error.GenerateErrorReport());
        
    }
    
    //Button Funktionen zum einloggen, ausloggen, registrieren
    
    public void OnClickLogin()
    {
        var request = new LoginWithPlayFabRequest { Username = username, Password =  userPassword};
        PlayFabClientAPI.LoginWithPlayFab(request,OnLoginSuccess,OnLoginFailure);
    }

    public void OnClickLogout()
    {
        PlayFabClientAPI.ForgetAllCredentials();
    }
    
    public void OnClickRegister()
    {
        if (CheckInputsValid())
        {
            var registerRequest = new RegisterPlayFabUserRequest { Username = username, Password = userPassword, RequireBothUsernameAndEmail = false};
            PlayFabClientAPI.RegisterPlayFabUser(registerRequest,OnRegisterSuccess,OnRegisterFailure);
        }
        else
        {
            Debug.Log("Fehler");
            Debug.Log("Username: "+username);
            Debug.Log("Passwort: "+userPassword);
        }
       
    }
    
    //Getter des UI Inputs
    public void GetUsername(string usernameIn)
    {
        username = usernameIn;
        Debug.Log("Input:" +usernameIn);
        Debug.Log("Username = "+ username);
    }

    public void GetUserPassword(string userPasswordIn)
    {
        userPassword = userPasswordIn;
    }

    private Boolean CheckInputsValid()
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userPassword))
        {
            return false;
        }

        return true;
    }

    private void showPopup(string text)
    {
         popupText.SetText(text + " Failure");
        loginPanel.SetActive(false);
        popupPanel.SetActive(true);
    }

    public void hidePopup()
    {
        loginPanel.SetActive(true);
        popupPanel.SetActive(false);
    }
    #endregion Login
}
