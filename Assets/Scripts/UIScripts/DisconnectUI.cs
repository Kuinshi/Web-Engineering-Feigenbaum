using TMPro;
using UnityEngine;

namespace UIScripts
{
    public class DisconnectUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        public void CallDisconnect(string reason)
        {
            text.text = reason;
            gameObject.SetActive(true);
        }
    }
}