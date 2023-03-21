using System.Collections;
using UnityEngine;

namespace Characters.Jaeger
{
    public class GunEffect : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip shotSfx, reloadSfx, noAmmoSfx;
        [SerializeField] private GameObject muzzleFlash;

        
        // Particle System
        
        
        public void PlayEffect()
        {
            audioSource.clip = shotSfx;
            audioSource.Play();
            StartCoroutine(FlashMuzzle());
        }

        private IEnumerator FlashMuzzle()
        {
            muzzleFlash.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            muzzleFlash.SetActive(false);
        }

        public void PlayReloadSound()
        {
            audioSource.clip = reloadSfx;
            audioSource.Play();
        }

        public void PlayNoAmmoSound()
        {
            audioSource.clip = noAmmoSfx;
            audioSource.Play();
        }
    }
}
