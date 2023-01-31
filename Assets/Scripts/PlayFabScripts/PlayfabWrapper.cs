using Manager;
using TMPro;
using UnityEngine;

namespace PlayFabScripts
{
    public class PlayfabWrapper : MonoBehaviour
    {

        public GameObject loginPanel;
        public GameObject popupPanel;
        public TextMeshProUGUI popupHeaderTxt;
        public TextMeshProUGUI popupContentTxt;

        [SerializeField] private TMP_InputField userNameField;
        [SerializeField] private TMP_InputField passwordField;
        [SerializeField] private GameObject lobbyLogIn;
        
        

        private void Start()
        {
            PlayFabManager.Instance.PlayfabWrapper = this;
        }

        public void OnClickRegister()
        {
            PlayFabManager.Instance.Register(userNameField.text, passwordField.text);
        }
        
        public void OnClickLogIn()
        {
            PlayFabManager.Instance.Login(userNameField.text, passwordField.text);
        }

        public void ShowPopup(int errorNr)
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
                    popupContentTxt.SetText("Password too short. 6 Character needed");
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

        public void HidePopup()
        {
            loginPanel.SetActive(true);
            popupPanel.SetActive(false);
        }

        public void OnScucessCallback()
        {
            // ToDo
            lobbyLogIn.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
