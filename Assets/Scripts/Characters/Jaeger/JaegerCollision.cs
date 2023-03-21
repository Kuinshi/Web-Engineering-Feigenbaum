using Fusion;
using Networking;
using UnityEngine;

namespace Characters.Jaeger
{
    public class JaegerCollision : NetworkBehaviour
    {
        [SerializeField] private Shooting shooting;
        [SerializeField] private CharacterMover mover;
        
        
        private void OnTriggerStay(Collider other)
        {
            if (HasInputAuthority)
            {
                if (other.CompareTag("AmmoPack"))
                {
                    shooting.Reload();
                    other.GetComponent<RemoveParachute>().Use();
                }

                if (other.CompareTag("FuelCrate"))
                {
                    mover.FillFuel(); // ToDo: Missing SFX?
                    other.GetComponent<RemoveParachute>().Use();
                }
            }
        }
    }
}
