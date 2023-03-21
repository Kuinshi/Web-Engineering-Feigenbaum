using System.Collections;
using System.Collections.Generic;
using Characters.Titan;
using Fusion;
using Networking;
using TMPro;
using UnityEngine;

namespace Characters.Jaeger
{
    public class Shooting : NetworkBehaviour
    {
        [SerializeField] private int currentAmmo;
        [SerializeField] private int maxAmmo;
        [SerializeField] private float timePerShot;
        [SerializeField] private int baseDamage;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private TextMeshProUGUI ammoText;

        [SerializeField] private List<GunEffect> gunEffects = new List<GunEffect>();
        
        

        private bool cooldown;

        private void Update()
        {
            if (cooldown)
                return;
            
            if (currentAmmo <= 0)
            {
                Rpc_ShotEffects(2);
                StartCoroutine(ShotCooldown());
                return;
            }
            
            if (Input.GetMouseButton(0))
            {
                Shoot();
                StartCoroutine(ShotCooldown());
            }
        }

        private void Shoot()
        {
            currentAmmo -= 1;
            UpdateAmmoText();
            
            Rpc_ShotEffects(0);
            
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if(Physics.Raycast(ray, out RaycastHit hit, 10000, layerMask))
            {
                Debug.Log("Shot hit: " + hit.collider.gameObject.name);
                var damageZone = hit.collider.GetComponent<DamageZone>();
                if (damageZone == null)
                    return;

                int damage = baseDamage * damageZone.damageMultiplier;
                foreach (var po in PlayerRegistry.Titan)
                {
                    po.TakeDamage(damage);
                }
            }
        }
        
        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        void Rpc_ShotEffects(int effectType)
        {
            if (effectType == 0)
            {
                foreach (var gunEffect in gunEffects)
                {
                    gunEffect.PlayEffect();
                }
            }
            
            if (effectType == 1)
            {
                foreach (var gunEffect in gunEffects)
                {
                    gunEffect.PlayReloadSound();
                }
            }
            
            if (effectType == 2)
            {
                foreach (var gunEffect in gunEffects)
                {
                    gunEffect.PlayNoAmmoSound();
                }
            }
        }

        private IEnumerator ShotCooldown()
        {
            cooldown = true;
            yield return new WaitForSeconds(timePerShot);
            cooldown = false;
        }

        public void Reload()
        {
            Rpc_ShotEffects(1);
            currentAmmo = maxAmmo;
            UpdateAmmoText();
        }

        private void UpdateAmmoText()
        {
            ammoText.text = currentAmmo + "/" + maxAmmo;
        }
    }
}
