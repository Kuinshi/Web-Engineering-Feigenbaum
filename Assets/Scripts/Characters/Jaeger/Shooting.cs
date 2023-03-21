using System;
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
        [SerializeField] private Camera fpsCamera;
        

        [SerializeField] private GunEffect firstPersonGunEffect;
        [SerializeField] private GunEffect thirdPersonGunEffect;

        
        
        

        private bool cooldown;

        private void Update()
        {
            if (!HasInputAuthority)
                return;
            
            if (cooldown)
                return;
            
            StartCoroutine(ShotCooldown());
            
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
            {
                if (currentAmmo <= 0)
                {
                    Debug.Log("Play no Ammo sound.");
                    Rpc_ShotEffects(2);
                }
                else
                {
                    Shoot();
                }
            }
        }

        private void Shoot()
        {
            currentAmmo -= 1;
            UpdateAmmoText();
            
            Rpc_ShotEffects(0);
            
            Ray ray = fpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
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
            GunEffect gunEffect = HasInputAuthority ? firstPersonGunEffect : thirdPersonGunEffect;
            
            if (effectType == 0)
            {
                if(gunEffect.gameObject.activeInHierarchy)
                        gunEffect.PlayEffect();
            }
            
            if (effectType == 1 && HasInputAuthority)
            {
                if(gunEffect.gameObject.activeInHierarchy)
                        gunEffect.PlayReloadSound();
            }
            
            if (effectType == 2 && HasInputAuthority)
            {
                if(gunEffect.gameObject.activeInHierarchy)
                        gunEffect.PlayNoAmmoSound();
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

        /*
        private void OnCollisionEnter(Collision other)
        {
            if (!HasInputAuthority)
            {
                return;
            }
            
            if(other.gameObject.CompareTag("AmmoPack"))
            {
                Reload();
                other.collider.GetComponent<RemoveParachute>().Use();
            }
        }
        */
    }
}
