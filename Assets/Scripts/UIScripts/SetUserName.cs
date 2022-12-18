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

        public void UpdateName()
        {
            LocalPlayerData.userName = _inputField.text;
        }
    }
}
