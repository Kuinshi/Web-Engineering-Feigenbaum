using System;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.EconomyModels;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;



public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager PFM;

    private string username;
    private string userPassword;
    private string myId;

    public GameObject loginPanel;

    public GameObject popupPanel;

    public TextMeshProUGUI popupHeaderTxt;

    public TextMeshProUGUI popupContentTxt;
    
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
        PlayerPrefs.SetString("PLAYERID",myId);
        GoToShop();
        Debug.Log(myId+" wurde eingeloggt");
        Debug.Log("Username = "+username);
    }

    //to do: zwischen errors differenzieren
    private void OnLoginFailure(PlayFabError error)
    {
        Debug.Log("Login fehlgeschlagen");
        showPopup(1);
    }
    
    
    //Neues Profil wird angelegt
    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        PlayerPrefs.SetString("USERNAME",username);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        myId = result.PlayFabId;
        
        showPopup(4);
        
        Debug.Log(myId+" wurde registriert");
        Debug.Log("Username = "+username);

    }
    
    private void OnRegisterFailure(PlayFabError error)
    {   
        if (userPassword.Length < 10)
        {
            showPopup(3);
        }
        else
        {
            showPopup(2);
        }
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

    private void showPopup(int errorNr)
    {
        //Alle moeglichen Fehler
        //  0: Login - falsches Passwort
        //  1: Login - Email existiert nicht
        //  2: Registrierung - Email schon vergeben
        //  3: R - Passwort zu kurz
        //  4: R- Success
        switch (errorNr)
        {
            case 0:
                popupHeaderTxt.SetText("Login Failure");
                popupContentTxt.SetText("Wrong password");
                break;
            case 1:
                popupHeaderTxt.SetText("Login Failure");
                popupContentTxt.SetText("Email not found.  Please register.");
                break;
            case 2:
                popupHeaderTxt.SetText("Register Failure");
                popupContentTxt.SetText("Email is already used");
                break;
            case 3:
                popupHeaderTxt.SetText("Register Failure");
                popupContentTxt.SetText("Password too short. X Character needed");
                break;
            case 4:
                popupHeaderTxt.SetText("Register Success");
                popupContentTxt.SetText("You can login now");
                break;
            default:
                popupHeaderTxt.SetText("Error");
                popupContentTxt.SetText("Error");
                break;
        }
        loginPanel.SetActive(false);
        popupPanel.SetActive(true);
    }

    public void hidePopup()
    {
        loginPanel.SetActive(true);
        popupPanel.SetActive(false);
    }

    public void GoToShop()
    {
        SceneManager.LoadScene(3);
    }
    
    #endregion Login

   
}
