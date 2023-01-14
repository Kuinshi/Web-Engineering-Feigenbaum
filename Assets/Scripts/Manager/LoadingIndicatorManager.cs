using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Manager
{
    public class LoadingIndicatorManager : MBSingelton<LoadingIndicatorManager>
    {
        [SerializeField] private GameObject canvas;
        [SerializeField] private RawImage background;
        
    
        public void StartLoading(float backgroundAlpha = 0.9f)
        {
            background.color = new Color(0, 0, 0, backgroundAlpha);
            canvas.SetActive(true);
        }

        public void StopLoading()
        {
            canvas.SetActive(false);
        }
    }
}
