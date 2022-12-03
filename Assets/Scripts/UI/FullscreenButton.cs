using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FullscreenButton : MonoBehaviour
    {
        [SerializeField] private Image buttonImage;
        [SerializeField] private Sprite goFullscreen;
        [SerializeField] private Sprite leaveFullscreen;

        public void Fullscreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
            buttonImage.sprite = Screen.fullScreen ? leaveFullscreen : goFullscreen;
        }
        
    }
}
