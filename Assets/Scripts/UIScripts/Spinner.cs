using UnityEngine;

namespace UIScripts
{
    public class Spinner : MonoBehaviour
    {
        [SerializeField] private float rotSpeed;
        
        
        // Update is called once per frame
        private void Update()
        {
            transform.Rotate(Vector3.forward, Time.deltaTime * rotSpeed);
        }
    }
}
