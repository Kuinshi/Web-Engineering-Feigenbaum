using Manager;
using Networking;
using TMPro;
using UnityEngine;

namespace UIScripts
{
    public class SetUserName : MonoBehaviour
    {
        private TMP_InputField _inputField;

        private void Awake()
        {
            _inputField = GetComponent<TMP_InputField>();
        }

        private void Start()
        {
            _inputField.text = PlayFabManager.Username;
            UpdateName();
        }

        public void UpdateName()
        {
            LocalPlayerData.userName = _inputField.text;
        }
    }
}
