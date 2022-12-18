using UnityEngine;
using Util;

namespace Manager
{
    public class LoadingIndicatorManager : MBSingelton<LoadingIndicatorManager>
    {
        [SerializeField] private GameObject canvas;
    
        public void StartLoading()
        {
            canvas.SetActive(true);
        }

        public void StopLoading()
        {
            canvas.SetActive(false);
        }
    }
}
