using UnityEngine;

namespace Characters.Jaeger
{
    public class GunEffect : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip shotSfx, reloadSfx, noAmmoSfx;
        // Particle System
        
        
        public void PlayEffect()
        {
            audioSource.clip = shotSfx;
            audioSource.Play();
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
