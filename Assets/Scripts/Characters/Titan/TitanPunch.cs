using System.Collections;
using Characters.Jaeger;
using Fusion;
using UnityEngine;

namespace Characters.Titan
{
    public class TitanPunch : NetworkBehaviour
    {
        [SerializeField] private int damageMultiplier;
        [SerializeField] private float minPunchMagnitude;
        
        
        private Vector3 currentPos;
        private Vector3 lastPos;
        private Vector3 secondLastPos;
        private Vector3 thirdLastPos;
        private Vector3 fourthLastPos;
        private Vector3 punchMagnitude;
        private bool punchCooldown;
        
        
        private void FixedUpdate()
        {
            if (!HasInputAuthority)
                return;

            fourthLastPos = thirdLastPos;
            thirdLastPos = secondLastPos;
            secondLastPos = lastPos;
            lastPos = currentPos;
            currentPos = transform.position;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!HasInputAuthority)
                return;

            if (punchCooldown)
                return;
            
            if (other.gameObject.CompareTag("Jaeger"))
            {
                CharacterMover enemy = other.gameObject.GetComponent<CharacterMover>();
                if(enemy != null)
                    Punch(enemy);
            }
        }

        private void Punch(CharacterMover enemy)
        {
            Vector3 punchVector = currentPos - fourthLastPos;
            
            if (Mathf.Abs(punchVector.magnitude) < minPunchMagnitude)
            {
                Debug.Log("Punch not fast enough with Magnitude of: " + Mathf.Abs(punchVector.magnitude));
                return;
            }
            
            Vector3 punchStrength = punchVector * damageMultiplier;
            enemy.TitanHit(punchStrength, damageMultiplier);
            StartCoroutine(Cooldown());
        }

        private IEnumerator Cooldown()
        {
            punchCooldown = true;
            yield return new WaitForSeconds(0.15f);
            punchCooldown = false;
        }
    }
}
