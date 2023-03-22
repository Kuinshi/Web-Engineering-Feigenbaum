using System.Collections.Generic;
using UnityEngine;

namespace Characters.Titan
{
    public class GrabTrigger : MonoBehaviour
    {
        public List<Rigidbody> grabbableObjects = new List<Rigidbody>();
        
        private void OnTriggerEnter(Collider other)
        {
            var grabbable = other.GetComponent<Grabbable>();
            if (grabbable == null)
                return;

            var rb = grabbable.GetComponent<Rigidbody>();
            grabbableObjects.Add(rb);
        }

        private void OnTriggerExit(Collider other)
        {
            var rb = other.GetComponent<Rigidbody>();
            if (rb != null && grabbableObjects.Contains(rb))
                grabbableObjects.Remove(rb);
        }
    }
}
