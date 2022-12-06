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
            PlayerData.userName = _inputField.text;
        }
    }
}
