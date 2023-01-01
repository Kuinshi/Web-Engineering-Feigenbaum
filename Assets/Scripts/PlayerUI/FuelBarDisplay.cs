using Characters.Jaeger;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerUI
{
    public class FuelBarDisplay : MonoBehaviour
    {
        [SerializeField] private CharacterMover characterMover;
        [SerializeField] private Image barFill;
        

        private void Start()
        {
            characterMover.OnFuelChanged += UpdateProgressBar;
        }

        private void UpdateProgressBar(float value)
        {
            barFill.fillAmount = value / characterMover.maxFuel;
            if (value == -1)
                barFill.fillAmount = 1;
        }

        private void OnDestroy()
        {
            characterMover.OnFuelChanged -= UpdateProgressBar;
        }
    }
}
